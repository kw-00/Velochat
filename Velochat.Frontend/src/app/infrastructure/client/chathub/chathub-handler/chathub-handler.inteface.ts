import type { Invitation, ChatMessage } from "@/app/infrastructure/models";

export type ChatHubEventHandlerMap = {
    roomClosed: (roomId: number) => void;
    invited: (invitation: Invitation) => void;
    kicked: (roomId: number) => void;
    messageReceived: (message: ChatMessage) => void;
}


export interface IChatHubHandler {
    addEventListener<T extends keyof ChatHubEventHandlerMap>(
        event: T, handler: ChatHubEventHandlerMap[T]
    ): void;


    removeEventListener<T extends keyof ChatHubEventHandlerMap>(
        event: T, handler: ChatHubEventHandlerMap[T]
    ): void;

    _roomClosed: ChatHubEventHandlerMap["roomClosed"];
    _invited: ChatHubEventHandlerMap["invited"];
    _kicked: ChatHubEventHandlerMap["kicked"];
    _messageReceived: ChatHubEventHandlerMap["messageReceived"];
}