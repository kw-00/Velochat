import type { User } from "../models";
import { getApiResponse, type ApiResponse } from "./response";

export type Credentials = {
    login: string;
    password: string;
}

export class IdentityClient {
    baseUrl: string;
    constructor(serverUrl: string) {
        this.baseUrl = `${serverUrl}/identity`;
    } 
    async registerAsync(credentials: Credentials): Promise<ApiResponse<User>> {
        const response = await fetch(`${this.baseUrl}/register`,  {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials)
        });

        return getApiResponse<User>(response);
    }
    async logInAsync(credentials: Credentials): Promise<ApiResponse<User>> {
        const response = await fetch(`${this.baseUrl}/login`,  {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials)
        });

        return getApiResponse(response);
    }
    async refreshTokenAsync(): Promise<ApiResponse<void>> {
        const response = await fetch(`${this.baseUrl}/refresh-token`,  {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });
        return getApiResponse(response);
    }
    
    async logOutAsync(): Promise<ApiResponse<void>> {
        const response = await fetch(`${this.baseUrl}/log-out`,  {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });
        return getApiResponse(response);
    }
}