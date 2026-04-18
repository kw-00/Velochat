import type { Identity } from "../../models";
import { getApiResponse, type ApiResponse } from "../response";
import type { Credentials, IIdentityClient } from "./identity-client.interface";

const identityEndpointUrl = `${import.meta.env.BACKEND_URL}`;

export class IdentityClient implements IIdentityClient {
    async registerAsync(credentials: Credentials): Promise<ApiResponse<Identity>> {
        const response = await fetch(`${identityEndpointUrl}/register`,  {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials)
        });

        return getApiResponse<Identity>(response);
    }
    async logInAsync(credentials: Credentials): Promise<ApiResponse<Identity>> {
        const response = await fetch(`${identityEndpointUrl}/login`,  {
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
        const response = await fetch(`${identityEndpointUrl}/refresh-token`,  {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });
        return getApiResponse(response);
    }
    async logOutAsync(): Promise<ApiResponse<void>> {
        const response = await fetch(`${identityEndpointUrl}/log-out`,  {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });
        return getApiResponse(response);
    }
}