import * as SignalR from "@microsoft/signalr";
import { FriendshipHubClient } from "./hub/friendship";
import { MessagingHubClient } from "./hub/messaging";
import { RoomsHubClient } from "./hub/rooms";
import { ServerEvents } from "./hub/server-events";
import { AuthClient } from "./rest/auth-client";
import { RealtimeConnection } from "./hub/realtime-connection";
import { FriendshipManager } from "./friendship-manager";
import { MessageManager } from "./message-manager";
import { RoomManager } from "./room-manager";




const serverUrl = "http://localhost:5000";


export class ServerInterface {
    static readonly singleton = new ServerInterface();

    readonly connection = new RealtimeConnection(
        new SignalR.HubConnectionBuilder()
            .withUrl(`${serverUrl}/hub`)
            .build()
    );

    private _serverEvents = new ServerEvents(this.connection);
    private _friendshipHubClient = new FriendshipHubClient(this.connection);
    private _roomsHubClient = new RoomsHubClient(this.connection);
    private _messagingHubClient = new MessagingHubClient(this.connection);

    readonly auth = new AuthClient(serverUrl);
    readonly friendship = new FriendshipManager(
        this.connection, 
        this._serverEvents, 
        this._friendshipHubClient
    );
    readonly rooms = new RoomManager(
        this.connection, 
        this._serverEvents,
        this._roomsHubClient
    );
    readonly messaging = new MessageManager(
        this.connection, 
        this._serverEvents,
        this._messagingHubClient
    );

    async startRealtimeSessionAsync() {
        try {
            await this.connection.startAsync();
        } catch (err) {
            if (err instanceof Error && err.message.includes("401")) {
                await this.auth.refreshSessionAsync();
                await this.connection.startAsync();
            }
        }
    }

    async stopRealtimeSessionAsync() {
        await this.connection.stopAsync();
    }
}


