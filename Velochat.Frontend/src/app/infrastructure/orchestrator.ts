import type { Credentials, IUserClient } from "./client/user/user-client.interface";

import type { User } from "./models";
import type { ApiResponse } from "./client/response";
import type { IUserStore } from "./data-stores/user-store/user-store.interface";
import type { IInvitationStore } from "./data-stores/invitation-store/invitation-store.interface";
import type { IMultiRoomMessageStore } from "./data-stores/message-store/message-store.interface";
import type { IChatHubClient } from "./client/chathub/chathub-client.interface";
import type { IRoomStore } from "./data-stores/room-store/room-store.interface";
import { UserClient } from "./client/user/user-client";
import { ChatHubClient } from "./client/chathub/chathub-client";
import { ChatHubHandler } from "./client/chathub/chathub-handler/chathub-handler";
import { ChatHubInit } from "./client/chathub/chathub-init/chathub-init";
import { ChatHubMembers } from "./client/chathub/chathub-members/chathub-members";
import { ChatHubMessages } from "./client/chathub/chathub-messages/chathub-messages";
import { ChatHubRooms } from "./client/chathub/chathub-rooms/chathub-rooms";
import { UserStore } from "./data-stores/user-store/user-store";
import { InvitationStore } from "./data-stores/invitation-store/invitation-store";
import { RoomStore } from "./data-stores/room-store/room-store";

import * as SignalR from "@microsoft/signalr";
import { MultiRoomMessageStore } from "./data-stores/message-store/message-store";


class Orchestrator {
    acceptsMessagesFromStream: boolean = true;

    private _userClient: IUserClient;
    
    private _chatHubClient: IChatHubClient;
    
    
    private _invitationStore: IInvitationStore;
    private _roomStore: IRoomStore;
    private _messageStore: IMultiRoomMessageStore;
    private _userStore: IUserStore; 

    get userClient(): IUserClient {
        return this._userClient;
    }
    get chatHubClient(): IChatHubClient {
        return this._chatHubClient;
    }
    get invitationStore(): IInvitationStore {
        return this._invitationStore;
    }
    get roomStore(): IRoomStore {
        return this._roomStore;
    }
    get messageStore(): IMultiRoomMessageStore {
        return this._messageStore;
    }
    get userStore(): IUserStore {
        return this._userStore;
    }

    constructor(
        userClient: IUserClient,
        chatHubClient: IChatHubClient,
        invitationStore: IInvitationStore,
        roomStore: IRoomStore,
        messageStore: IMultiRoomMessageStore,
        userStore: IUserStore
    ) {
        this._userClient = userClient;
        this._chatHubClient = chatHubClient;
        this._invitationStore = invitationStore;
        this._roomStore = roomStore;
        this._messageStore = messageStore;
        this._userStore = userStore;
    }


    setUpSignalREventHandlers() {
        this.chatHubClient.handler.addEventListener("invited", (invitation) => {
            this.invitationStore.add(invitation);
        });
        
        this.chatHubClient.handler.addEventListener("kicked", (roomId) => {
            this.roomStore.remove(roomId);
        });

        this.chatHubClient.handler.addEventListener("messageReceived", (message) => {
            if (this.acceptsMessagesFromStream) {
                this.messageStore.append(message);
            }
        });

        this.chatHubClient
            .handler
            .addEventListener("reconnected", () => this._reconcileStateWithServer());
    }

    async registerAndConnect(credentials: Credentials): Promise<void> {
        return this._authenticateAndConnect(
            credentials, (credentials) => this.userClient.registerAsync(credentials)
        );
    }
    
    async logInAndConnect(credentials: Credentials): Promise<void> {
        return this._authenticateAndConnect(
            credentials, (credentials) => this.userClient.logInAsync(credentials)
        );
    }

    async connect() {
        await this.chatHubClient.connect();
        await this._reconcileStateWithServer();
    }
    
    
    private async _authenticateAndConnect(
        credentials: Credentials,
        method: (credentials: Credentials) => Promise<ApiResponse<User>>
    
    ): Promise<void> {
    
        const authResult = await method(credentials);
        if (!authResult.success) {
            throw new Error(authResult.message);
        }
        this.userStore.overwrite(authResult.data);
        await this.connect();
    }

    private async _reconcileStateWithServer() {
        const initialData = await this
            .chatHubClient
            .init
            .getInitialChatInformation();

        this.invitationStore.overwrite(initialData.invitations);
        this.roomStore.overwrite(initialData.rooms);
        const selectedRoomId = this.messageStore.selectedRoomId;
        if (selectedRoomId !== null) {
            const newMessages = await this
                .chatHubClient
                .messages
                .getRecentMessages(selectedRoomId);

            this.messageStore.reset(newMessages);
        }
    }


}


export const OrchestratorInstance = new Orchestrator(
    new UserClient(),
    new ChatHubClient(
        new SignalR.HubConnectionBuilder()
            .withUrl(`${import.meta.env.BACKEND_URL ?? "http://localhost:5000"}/chat-hub`) //TODO remove fallback
            .build(),
        ChatHubInit,
        ChatHubRooms,
        ChatHubMembers,
        ChatHubMessages,
        ChatHubHandler
    ),
    new InvitationStore(),
    new RoomStore(),
    new MultiRoomMessageStore(300, 100),
    new UserStore()
);



