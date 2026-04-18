import * as SignalR from "@microsoft/signalr";
import { ChatHubInit } from "./chathub-init/chathub-init";
import { ChatHubMembers } from "./chathub-members/chathub-members";
import { ChatHubMessages } from "./chathub-messages/chathub-messages";
import { ChatHubRooms } from "./chathub-rooms/chathub-rooms";
import type { IChatHubHandler } from "./chathub-handler/chathub-handler.inteface";
import ChatHubHandler from "./chathub-handler/chathub-handler";

class ChatHubClient {
    private _connection: SignalR.HubConnection;
    private _init: ChatHubInit;
    private _rooms: ChatHubRooms;
    private _members: ChatHubMembers;
    private _messages: ChatHubMessages;
    private _handler: IChatHubHandler;
    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
        this._init = new ChatHubInit(connection);
        this._rooms = new ChatHubRooms(connection);
        this._members = new ChatHubMembers(connection);
        this._messages = new ChatHubMessages(connection);
        this._handler = new ChatHubHandler(connection);
    }

    async connect(): Promise<void> {
        await this._connection.start();
    }

    get init(): ChatHubInit { return this._init; }
    get rooms(): ChatHubRooms { return this._rooms; }
    get members(): ChatHubMembers { return this._members; }
    get messages(): ChatHubMessages { return this._messages; }
    get handler(): IChatHubHandler { return this._handler; }
}

const globalChatHubClient = new ChatHubClient(
    new SignalR.HubConnectionBuilder().withUrl("/chat-hub").build()
);

export default globalChatHubClient;