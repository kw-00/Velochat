import * as SignalR from "@microsoft/signalr";
import { FriendshipHubClient } from "./hub/friendship";
import { MessagingHubClient } from "./hub/messaging";
import { RoomsHubClient } from "./hub/rooms";
import { ServerEvents } from "./hub/server-events";
import { IdentityClient } from "./rest/identity-client";
import { RealtimeConnection } from "./hub/realtime-connection";
import { FriendshipManager } from "./friendship-manager";
import { MessageManager } from "./message-manager";
import { RoomManager } from "./room-manager";







export class ServerInterface {
    static readonly singleton = new ServerInterface();

    private _connection = new RealtimeConnection(
        new SignalR.HubConnectionBuilder()
            .withUrl(`${import.meta.env.BASE_URL}/hub`)
            .build()
    );

    private _serverEvents = new ServerEvents(this._connection);
    private _friendshipHubClient = new FriendshipHubClient(this._connection);
    private _roomsHubClient = new RoomsHubClient(this._connection);
    private _messagingHubClient = new MessagingHubClient(this._connection);

    readonly auth = new IdentityClient();
    readonly friendship = new FriendshipManager(
        this._connection, 
        this._serverEvents, 
        this._friendshipHubClient
    );
    readonly rooms = new RoomManager(
        this._connection, 
        this._serverEvents,
        this._roomsHubClient
    );
    readonly messaging = new MessageManager(
        this._connection, 
        this._serverEvents,
        this._messagingHubClient
    );
}


