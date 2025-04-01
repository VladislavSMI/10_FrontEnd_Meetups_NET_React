import { IUser } from "./user";

export interface IProfile {
  userName: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: IPhoto[];
}

export class IProfile implements IProfile {
  constructor(user: IUser) {
    this.userName = user.userName;
    this.displayName = user.displayName;
    this.image = user.image;
  }
}

export interface IPhoto {
  id: string;
  url: string;
  isMain: boolean;
}
