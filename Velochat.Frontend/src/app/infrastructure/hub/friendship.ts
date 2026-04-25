import type { User } from "../models";
import { HubClient } from "./hub-client";



const DISPATCHER_NAME = "Friendship";

export class FriendshipHubClient extends HubClient {
    async getFriendsAsync() {
        return await this.invokeAsync<User[]>(
            DISPATCHER_NAME, "GetFriends"
        );
    }

    async getRequestsAsync() {
        return await this.invokeAsync<User[]>(
            DISPATCHER_NAME, "GetRequests"
        );
    }

    async requestAsync(userId: number) {
        return await this.invokeNoContentAsync(
            DISPATCHER_NAME, "Request", userId
        );
    }

    async acceptAsync(userId: number) {
        return await this.invokeNoContentAsync(
            DISPATCHER_NAME, "Accept", userId
        );
    }

    async declineAsync(userId: number) {
        return await this.invokeNoContentAsync(
            DISPATCHER_NAME, "Decline", userId
        );
    }
}