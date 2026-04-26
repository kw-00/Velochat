import * as SignalR from "@microsoft/signalr";
import { FriendshipHubClient } from "./hub/friendship";
import { MessagingHubClient } from "./hub/messaging";
import { RoomsHubClient } from "./hub/rooms";
import { ServerEvents } from "./hub/server-events";
import { IdentityClient } from "./rest/identity-client";
import { RealtimeConnection } from "./hub/realtime-connection";







export class ServerInterface {
    static readonly singleton = new ServerInterface();

    private _connection = new RealtimeConnection(
        new SignalR.HubConnectionBuilder()
            .withUrl(`${import.meta.env.BASE_URL}/hub`)
            .build()
    );

    readonly serverEvents = new ServerEvents(this._connection);
    readonly messaging = new MessagingHubClient(this._connection);
    readonly rooms = new RoomsHubClient(this._connection);
    readonly friendship = new FriendshipHubClient(this._connection);
    readonly identity = new IdentityClient();
}


