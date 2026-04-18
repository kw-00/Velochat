import type { Room } from "@/app/infrastructure/models";
import * as SignalR from "@microsoft/signalr";
import type { IChatHubRooms } from "./chathub-rooms.interface";

export class ChatHubRooms implements IChatHubRooms {
    private _connection: SignalR.HubConnection;

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    async createRoom(name: string): Promise<Room> {
        return this._connection.invoke("CreateRoom", name);
    }

    async destroyRoom(roomId: number): Promise<void> {
        return this._connection.invoke("DestroyRoom", roomId);
    }

    
    async joinRoom(roomId: number): Promise<Room> {
        return this._connection.invoke("JoinRoom", roomId);
    }

    async leaveRoom(roomId: number): Promise<void> {
        return this._connection.invoke("LeaveRoom", roomId);
    }

}