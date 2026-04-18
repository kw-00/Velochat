import * as SignalR from "@microsoft/signalr";
import { ChatHubInit } from "./chathub-init/chathub-init";
import { ChatHubMembers } from "./chathub-members/chathub-members";
import { ChatHubMessages } from "./chathub-messages/chathub-messages";
import { ChatHubRooms } from "./chathub-rooms/chathub-rooms";
import type { IChatHubHandler } from "./chathub-handler/chathub-handler.inteface";
import { ChatHubHandler } from "./chathub-handler/chathub-handler";
import type { IChatHubClient } from "./chathub-client.interface";
import type { IChatHubInit } from "./chathub-init/chathub-init.interface";
import type { IChatHubMembers } from "./chathub-members/chathub-members.interface";
import type { IChatHubMessages } from "./chathub-messages/chathub-messages.interface";
import type { IChatHubRooms } from "./chathub-rooms/chathub-rooms.interface";


type Constructor<T> = new (connection: SignalR.HubConnection) => T

export class ChatHubClient implements IChatHubClient {
    private _connection: SignalR.HubConnection;
    private _init: IChatHubInit;
    private _rooms: IChatHubRooms;
    private _members: IChatHubMembers;
    private _messages: IChatHubMessages;
    private _handler: IChatHubHandler;


    constructor(
        connection: SignalR.HubConnection,
        init: Constructor<ChatHubInit>,
        rooms: Constructor<ChatHubRooms>,
        members: Constructor<ChatHubMembers>,
        messages: Constructor<ChatHubMessages>,
        handler: Constructor<ChatHubHandler>

    ) {
        this._connection = connection;
        this._init = new init(connection);
        this._rooms = new rooms(connection);
        this._members = new members(connection);
        this._messages = new messages(connection);
        this._handler = new handler(connection);
    }
    
    get init(): IChatHubInit { return this._init; }
    get rooms(): IChatHubRooms { return this._rooms; }
    get members(): IChatHubMembers { return this._members; }
    get messages(): IChatHubMessages { return this._messages; }
    get handler(): IChatHubHandler { return this._handler; }

    async connect(): Promise<void> {
        await this._connection.start();
    }
}