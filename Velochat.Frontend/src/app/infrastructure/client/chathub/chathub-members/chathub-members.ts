import * as SignalR from "@microsoft/signalr";

import type { IChatHubMembers } from "./chathub-members.interface";

export class ChatHubMembers implements IChatHubMembers {
    private _connection: SignalR.HubConnection;

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    async invite(roomId: number, userId: number): Promise<void> {
        return this._connection.invoke("Invite", roomId, userId);
    }

    async revokeInvitation(roomId: number, userId: number): Promise<void> {
        return this._connection.invoke("RevokeInvitation", roomId, userId);
    }

    async kickMember(roomId: number, userId: number): Promise<void> {
        return this._connection.invoke("KickMember", roomId, userId);
    }
}