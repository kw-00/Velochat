import type { ChatMessage } from "../../models";

export interface IGlobalMessageStore {
    get selectedRoomId(): number | null;

    subscribe(callback: (messages: ChatMessage[]) => void): () => void;

    selectRoom(roomId: number): ChatMessage[];

    getMessages(): ChatMessage[];

    append(...messages: ChatMessage[]): void;

    prepend(...messages: ChatMessage[]): void;
}