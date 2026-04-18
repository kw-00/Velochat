import type { Identity } from "../../models";
import type { ApiResponse } from "../response";


export type Credentials = {
    login: string;
    password: string;
}

export interface IIdentityClient
{
    registerAsync(credentials: Credentials): Promise<ApiResponse<Identity>>;

    logInAsync(credentials: Credentials): Promise<ApiResponse<Identity>>;

    refreshTokenAsync(): Promise<ApiResponse<void>>;

    logOutAsync(): Promise<ApiResponse<void>>;
}