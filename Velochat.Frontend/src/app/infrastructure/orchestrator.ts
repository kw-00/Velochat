import type { Credentials, IIdentityClient } from "./client/identity/identity-client.interface";

import type { Identity } from "./models";
import type { ApiResponse } from "./client/response";
import type { IIdentityStore } from "./data-stores/identity-store/identity-store.interface";
import type { IInvitationStore } from "./data-stores/invitation-store/invitation-store.interface";
import type { IGlobalMessageStore } from "./data-stores/message-store/message-store.interface";
import type { IChatHubClient } from "./client/chathub/chathub-client.interface";
import type { IRoomStore } from "./data-stores/room-store/room-store.interface";
import { IdentityClient } from "./client/identity/identity-client";
import { ChatHubClient } from "./client/chathub/chathub-client";
import ChatHubHandler from "./client/chathub/chathub-handler/chathub-handler";
import { ChatHubInit } from "./client/chathub/chathub-init/chathub-init";
import { ChatHubMembers } from "./client/chathub/chathub-members/chathub-members";
import { ChatHubMessages } from "./client/chathub/chathub-messages/chathub-messages";
import { ChatHubRooms } from "./client/chathub/chathub-rooms/chathub-rooms";
import { IdentityStore } from "./data-stores/identity-store/identity-store";
import { InvitationStore } from "./data-stores/invitation-store/invitation-store";
import { RoomStore } from "./data-stores/room-store/room-store";

import * as SignalR from "@microsoft/signalr";
import { GlobalMessageStore } from "./data-stores/message-store/message-store";


class Orchestrator {
    acceptsMessagesFromStream: boolean = true;

    private _identityClient: IIdentityClient;
    
    private _chatHubClient: IChatHubClient;
    
    
    private _invitationStore: IInvitationStore;
    private _roomStore: IRoomStore;
    private _messageStore: IGlobalMessageStore;
    private _identityStore: IIdentityStore; 

    get identityClient(): IIdentityClient {
        return this._identityClient;
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
    get messageStore(): IGlobalMessageStore {
        return this._messageStore;
    }
    get identityStore(): IIdentityStore {
        return this._identityStore;
    }

    constructor(
        identityClient: IIdentityClient,
        chatHubClient: IChatHubClient,
        invitationStore: IInvitationStore,
        roomStore: IRoomStore,
        messageStore: IGlobalMessageStore,
        identityStore: IIdentityStore
    ) {
        this._identityClient = identityClient;
        this._chatHubClient = chatHubClient;
        this._invitationStore = invitationStore;
        this._roomStore = roomStore;
        this._messageStore = messageStore;
        this._identityStore = identityStore;
    }
    

    async registerAndConnect(credentials: Credentials): Promise<void> {
        return this._authenticateAndConnect(
            credentials, (credentials) => this.identityClient.registerAsync(credentials)
        );
    }
    
    async logInAndConnect(credentials: Credentials): Promise<void> {
        return this._authenticateAndConnect(
            credentials, (credentials) => this.identityClient.logInAsync(credentials)
        );
    }
    
    
    private async _authenticateAndConnect(
        credentials: Credentials,
        method: (credentials: Credentials) => Promise<ApiResponse<Identity>>
    
    ): Promise<void> {
    
        const authResult = await method(credentials);
        if (!authResult.success) {
            throw new Error(authResult.message);
        }
        this.identityStore.overwrite(authResult.data);
        this.chatHubClient.connect();

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
    }
}


export const OrchestratorInstance = new Orchestrator(
    new IdentityClient(),
    new ChatHubClient(
        new SignalR.HubConnectionBuilder().withUrl("/chat-hub").build(),
        ChatHubInit,
        ChatHubRooms,
        ChatHubMembers,
        ChatHubMessages,
        ChatHubHandler
    ),
    new InvitationStore(),
    new RoomStore(),
    new GlobalMessageStore(300, 100),
    new IdentityStore()
);



