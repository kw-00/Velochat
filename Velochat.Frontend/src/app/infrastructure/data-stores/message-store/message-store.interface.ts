import type { ChatMessage } from "../../models";

export interface IGlobalMessageStore {
    get selectedRoomId(): number | null;

    subscribeMessagesChanged(callback: (messages: ChatMessage[]) => void): () => void;

    subscribeRoomChanged(callback: (roomId: number) => void): () => void;

    selectRoom(roomId: number): ChatMessage[];

    getMessages(): ChatMessage[];

    append(...messages: ChatMessage[]): void;

    prepend(...messages: ChatMessage[]): void;

    reset(messages: ChatMessage[]): void;
}