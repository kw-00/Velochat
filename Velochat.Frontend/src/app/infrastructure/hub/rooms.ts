import type { Room } from "../models";
import { HubClient } from "./hub-client";



const DISPATCHER_NAME = "Rooms";

export class RoomsHubClient extends HubClient {
    async getRoomsAsync() {
        return await this.invokeAsync<Room[]>(
            DISPATCHER_NAME, "GetRooms"
        );
    }

    async createRoomAsync(name: string) {
        return await this.invokeAsync<Room>(
            DISPATCHER_NAME, "CreateRoom", name
        );
    }

    async leaveRoomAsync(roomId: number) {
        return await this.invokeNoContentAsync(
            DISPATCHER_NAME, "LeaveRoom", roomId
        );
    }

    async addUserAsync(roomId: number, userId: number) {
        return await this.invokeNoContentAsync(
            DISPATCHER_NAME, "AddUser", roomId, userId
        );
    }
}