import type { User } from "../models";
import { getApiResponse, type ApiResponse } from "./response";

const userEndpointUrl = `${import.meta.env.BACKEND_URL}`;

export type Credentials = {
    login: string;
    password: string;
}

export class IdentityClient {
    async registerAsync(credentials: Credentials): Promise<ApiResponse<User>> {
        const response = await fetch(`${userEndpointUrl}/register`,  {
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
        const response = await fetch(`${userEndpointUrl}/login`,  {
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
        const response = await fetch(`${userEndpointUrl}/refresh-token`,  {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });
        return getApiResponse(response);
    }
    
    async logOutAsync(): Promise<ApiResponse<void>> {
        const response = await fetch(`${userEndpointUrl}/log-out`,  {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });
        return getApiResponse(response);
    }
}