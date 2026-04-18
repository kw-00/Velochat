import type { ChatMessage } from "@/app/infrastructure/models";
import * as SignalR from "@microsoft/signalr";
import type { IChatHubMessages } from "./chathub-messages.interface";

export class ChatHubMessages implements IChatHubMessages{
    private _connection: SignalR.HubConnection;

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    async sendMessage(roomId: number, content: string): Promise<ChatMessage> {
        return this._connection.invoke("SendMessage", roomId, content);
    }

    async getMessagesBefore(roomId: number, before: number): Promise<ChatMessage[]> {
        return this._connection.invoke("GetMessagesBefore", roomId, before);
    }

    async getMessagesAfter(roomId: number, after: number): Promise<ChatMessage[]> {
        return this._connection.invoke("GetMessagesAfter", roomId, after);
    }

    async getRecentMessages(roomId: number): Promise<ChatMessage[]> {
        return this._connection.invoke("GetRecentMessages", roomId);
    }
}