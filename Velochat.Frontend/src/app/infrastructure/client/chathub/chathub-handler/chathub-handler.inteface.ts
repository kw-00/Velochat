import type { Invitation, ChatMessage } from "@/app/infrastructure/models";

export type ChatHubEventHandlerMap = {
    reconnected: () => void;
    roomClosed: (roomId: number) => void;
    invited: (invitation: Invitation) => void;
    kicked: (roomId: number) => void;
    messageReceived: (message: ChatMessage) => void;
}


export interface IChatHubHandler {
    addEventListener<T extends keyof ChatHubEventHandlerMap>(
        event: T, 
        handler: ChatHubEventHandlerMap[T]
    ): () => void;


    removeEventListener<T extends keyof ChatHubEventHandlerMap>(
        event: T, 
        handler: ChatHubEventHandlerMap[T]
    ): void;
}