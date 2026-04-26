import type { ChatMessage } from "../models";
import { HubClient } from "./hub-client";




export type GoNewerResponse = {
    messages: ChatMessage[];
    bottomReached: boolean;
}

export type GoOlderResponse = {
    messages: ChatMessage[];
    topReached: boolean;
}

export type SwitchRoomsResponse = {
    messages: ChatMessage[];
    isContinuity: boolean;
}

const DISPATCHER_NAME = "Messaging";

export class MessagingHubClient extends HubClient {

    async sendMessageAsync(content: string) {
        return await this.invokeNoContentAsync(
            DISPATCHER_NAME, "SendMessage", content
        );
    }

    async goOlderAsync(oldestMessageOnClient: number) {
        return await this.invokeAsync<GoOlderResponse>(
            DISPATCHER_NAME, "GoOlder", oldestMessageOnClient
        );
    }

    async goNewerAsync(newestMessageOnClient: number) {
        return await this.invokeAsync<GoNewerResponse>(
            DISPATCHER_NAME, "GoNewer", newestMessageOnClient
        );
    }

    async switchFocusAsync(roomId: number, newestMessageOnClient: number | null) {
        return await this.invokeAsync<SwitchRoomsResponse>(
            DISPATCHER_NAME, "SwitchFocus", roomId, newestMessageOnClient
        );
    }

    async zoneOutAsync() {
        return await this.invokeNoContentAsync(
            DISPATCHER_NAME, "ZoneOut"
        );
    }
}