import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { history } from "../../index";
import { ActivityFormValues, IActivity } from "../models/activity";
import { PaginatedResult } from "../models/pagination";
import { IPhoto, IProfile, IUserActivity } from "../models/profile";
import { IUser, IUserFormValues } from "../models/user";
import { store } from "../stores/store";

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.defaults.baseURL = import.meta.env.VITE_API_URL;

axios.interceptors.request.use((config) => {
  const token = store.commonStore.token;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(
  async (response) => {
    if (import.meta.env.DEV) await sleep(1000);
    const pagination = response.headers["pagination"];
    if (pagination) {
      response.data = new PaginatedResult(
        response.data,
        JSON.parse(pagination)
      );
      return response as AxiosResponse<PaginatedResult<any>>;
    }
    return response;
  },
  (error: AxiosError) => {
    const { data, status, config, headers } = error.response!;
    switch (status) {
      case 400:
        // There are two types of 400 responses: Bad Request and Validation Error.
        // If the response contains an "errors" object, it's considered a Validation Error, not a generic Bad Request.

        const { errors } = data;
        const { method } = config;

        if (typeof data === "string") {
          toast.error(data);
        }

        if (method === "get" && errors.hasOwnProperty("id")) {
          history.push("/not-found");
        }
        if (errors) {
          const modalStateErrors = [];
          for (const key in errors) {
            if (errors[key]) {
              modalStateErrors.push(errors[key]);
            }
          }
          throw modalStateErrors.flat();
        }
        break;
      case 401:
        if (
          status === 401 &&
          headers["www-authenticate"].startsWith('Bearer error="invalid_token"')
        ) {
          store.userStore.logout();
          toast.error("Session expired - please login again");
        }
        break;
      case 403:
        history.push("/not-found");
        break;
      case 404:
        history.push("/not-found");
        break;
      case 500:
        store.commonStore.setServerError(data);
        toast.error(`Error: ${data.message}`);
        history.push("/server-error");
        store.modalStore.closeModal();
        break;
    }
    return Promise.reject(error);
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Activities = {
  list: (params: URLSearchParams) =>
    axios
      .get<PaginatedResult<IActivity[]>>("/activities", { params })
      .then(responseBody),
  details: (id: string) => requests.get<IActivity>(`/activities/${id}`),
  create: (activity: ActivityFormValues) =>
    requests.post<void>("/activities", activity),
  update: (activity: ActivityFormValues) =>
    requests.put<void>(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.del<void>(`/activities/${id}`),
  attend: (id: string) => requests.post<void>(`/activities/${id}/attend`, {}),
};

const Account = {
  current: () => requests.get<IUser>("/account"),
  login: (user: IUserFormValues) =>
    requests.post<IUser>("/account/login", user),
  register: (user: IUserFormValues) =>
    requests.post<IUser>("/account/register", user),
  refreshToken: () => requests.post<IUser>("/account/refreshToken", {}),
};

const Profiles = {
  get: (username: string) => requests.get<IProfile>(`/profiles/${username}`),
  updateProfile: (profile: Partial<IProfile>) =>
    requests.put<void>(`/profiles`, profile),
  uploadPhoto: (file: Blob) => {
    let formData = new FormData();
    formData.append("File", file);
    return axios.post<IPhoto>("photos", formData, {
      headers: { "Content-type": "multipart/form-data" },
    });
  },
  setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
  deletePhoto: (id: string) => requests.del(`/photos/${id}`),
  updateFollowing: (username: string) =>
    requests.post(`/follow/${username}`, {}),
  listFollowings: (username: string, predicate: string) =>
    requests.get<IProfile[]>(`/follow/${username}?predicate=${predicate}`),
  listActivities: (username: string, predicate: string) =>
    requests.get<IUserActivity[]>(
      `/profiles/${username}/activities?predicate=${predicate}`
    ),
};

const agent = {
  Activities,
  Account,
  Profiles,
};

export default agent;
