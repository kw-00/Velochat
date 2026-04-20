import type { ChatMessage } from "../../models";


export type GlobalMessageStoreEventHandlers = {
    appended: (appended: ChatMessage[]) => void;
    prepended: (prepended: ChatMessage[]) => void;
    removedStart: (removed: ChatMessage[]) => void;
    removedEnd: (removed: ChatMessage[]) => void;
    reset: (newMessages: ChatMessage[]) => void;
    roomChanged: (roomId: number) => void;
}

export interface IMultiRoomMessageStore {
    get selectedRoomId(): number | null;

    addEventListener<T extends keyof GlobalMessageStoreEventHandlers>(
        event: T, 
        handler: GlobalMessageStoreEventHandlers[T]
    ): () => void

    removeEventListener<T extends keyof GlobalMessageStoreEventHandlers>(
        event: T, 
        handler: GlobalMessageStoreEventHandlers[T]
    ): void

    selectRoom(roomId: number): ChatMessage[];

    getMessages(): ChatMessage[];

    append(...messages: ChatMessage[]): void;

    prepend(...messages: ChatMessage[]): void;

    reset(messages: ChatMessage[]): void;
}