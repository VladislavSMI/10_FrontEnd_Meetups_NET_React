import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { makeAutoObservable, runInAction } from "mobx";
import { IChatComment } from "../models/comment";
import { store } from "./store";

export default class CommentStore {
  comments: IChatComment[] = [];
  hubConnection: HubConnection | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  createHubConnection = (activityId: string) => {
    if (store.activityStore.selectedActivity) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(process.env.REACT_APP_CHAT_URL + "?activityId=" + activityId, {
          accessTokenFactory: () => store.userStore.user?.token!,
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

      this.hubConnection
        .start()
        .catch((error) =>
          console.log("error establishing the connection: ", error)
        );

      this.hubConnection.on("LoadComments", (comments: IChatComment[]) => {
        runInAction(() => {
          comments.forEach((comment) => {
            //we have to manually add Z as we are not getting back from db Z at the end of time date string that indicates it is UTC time
            comment.createdAt = new Date(comment.createdAt + "Z");
          });
          this.comments = comments;
        });
      });

      this.hubConnection.on("ReceiveComment", (comment: IChatComment) => {
        runInAction(() => {
          // Here we don't have to add Z at the end, as it is coming from SignalR where Z is appended as the end
          comment.createdAt = new Date(comment.createdAt);
          this.comments.unshift(comment);
        });
      });
    }
  };

  stopHubConnection = () => {
    this.hubConnection
      ?.stop()
      .catch((error) => console.log("Error stopping connection: ", error));
  };

  clearComments = () => {
    this.comments = [];
    this.stopHubConnection();
  };

  addComment = async (values: any) => {
    values.activityId = store.activityStore.selectedActivity?.id;

    try {
      // "SendComment" has to exactly match the method name on our backend, it is like post request and we are sending "values" in the body. In our hubConnection we have method on "ReceiveComment" => we will get this back from our server and it will push it to our comment array
      await this.hubConnection?.invoke("SendComment", values);
    } catch (error) {
      console.log(error);
    }
  };
}
