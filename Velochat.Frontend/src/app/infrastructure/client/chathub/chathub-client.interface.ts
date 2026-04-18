import type { IChatHubHandler } from "./chathub-handler/chathub-handler.inteface";
import type { IChatHubInit } from "./chathub-init/chathub-init.interface";
import type { IChatHubMembers } from "./chathub-members/chathub-members.interface";
import type { IChatHubMessages } from "./chathub-messages/chathub-messages.interface";
import type { IChatHubRooms } from "./chathub-rooms/chathub-rooms.interface";


export interface IChatHubClient {
    get init(): IChatHubInit;
    get rooms(): IChatHubRooms;
    get members(): IChatHubMembers;
    get messages(): IChatHubMessages;
    get handler(): IChatHubHandler;

    connect(): Promise<void>;
}