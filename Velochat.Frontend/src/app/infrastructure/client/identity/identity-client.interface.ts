import type { User } from "../../models";
import type { ApiResponse } from "../response";


export type Credentials = {
    login: string;
    password: string;
}

export interface IUserClient
{
    registerAsync(credentials: Credentials): Promise<ApiResponse<User>>;

    logInAsync(credentials: Credentials): Promise<ApiResponse<User>>;

    refreshTokenAsync(): Promise<ApiResponse<void>>;

    logOutAsync(): Promise<ApiResponse<void>>;
}