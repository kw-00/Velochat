import * as SignalR from "@microsoft/signalr";

import type { IChatHubMembers } from "./chathub-members.interface";

export class ChatHubMembers implements IChatHubMembers {
    private _connection: SignalR.HubConnection;

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    async invite(roomId: number, identityId: number): Promise<void> {
        return await this._connection.invoke("Invite", roomId, identityId);
    }

    async revokeInvitation(roomId: number, identityId: number): Promise<void> {
        return await this._connection.invoke("RevokeInvitation", roomId, identityId);
    }

    async kickMember(roomId: number, identityId: number): Promise<void> {
        return await this._connection.invoke("KickMember", roomId, identityId);
    }
}