import * as SignalR from "@microsoft/signalr";
import type { InitialChatInformation } from "./chathub-init.interface";




export class ChatHubInit{
    private _connection: SignalR.HubConnection;

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    async getInitialChatInformation(): Promise<InitialChatInformation> {
        return this._connection.invoke("GetInitialChatInformation");
    }
}