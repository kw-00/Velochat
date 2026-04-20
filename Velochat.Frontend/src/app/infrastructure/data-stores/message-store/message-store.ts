import type { ChatMessage } from "../../models";
import type { GlobalMessageStoreEventHandlers as MultiRoomMessageStoreEventHandlers, IMultiRoomMessageStore } from "./message-store.interface";



export class RoomMessageStore {
    private _messages: ChatMessage[] = [];
    private _capacity: number;

    constructor(capacity: number) {
        this._capacity = capacity;
    }

    get messages(): ChatMessage[] {
        return this._messages;
    }

    append(...messages: ChatMessage[]): void {
        this._messages = this._messages.concat(messages);
    }

    prepend(...messages: ChatMessage[]): void {
        this._messages = messages.concat(this._messages);
    }

    reset(messages: ChatMessage[]): void {
        this._messages = messages;
    }

    cutOffExcessStart(): ChatMessage[] {
        if (this._messages.length > this._capacity) {
            return this._messages = this._messages.splice(
                0, this._messages.length - this._capacity
            );
        }
        return [];
    }

    cutOffExcessEnd(): ChatMessage[] {
        if (this._messages.length > this._capacity) {
            return this._messages = this._messages.splice(
                this._messages.length - this._capacity
            );
        }
        return [];
    }
}

export class MultiRoomMessageStore implements IMultiRoomMessageStore {
    _stores: Map<number, {store: RoomMessageStore, lastAccessed: number}> = new Map();
    _messagesPerRoomLimit: number;
    _roomLimit: number;

    _selectedRoomId: number | null = null;

    _subscribers: {
            [K in keyof MultiRoomMessageStoreEventHandlers]
            : Set<MultiRoomMessageStoreEventHandlers[K]>
        } = {
        appended: new Set<(appended: ChatMessage[]) => void>(),
        prepended: new Set<(prepended: ChatMessage[]) => void>(),
        removedStart: new Set<(removed: ChatMessage[]) => void>(),
        removedEnd: new Set<(removed: ChatMessage[]) => void>(),
        reset: new Set<(newMessages: ChatMessage[]) => void>(),
        roomChanged: new Set<(roomId: number) => void>()
    };

    get selectedRoomId(): number | null {
        return this._selectedRoomId;
    }

    constructor(messagesPerRoomLimit: number, roomLimit: number) {
        this._messagesPerRoomLimit = messagesPerRoomLimit;
        this._roomLimit = roomLimit;
    }

    addEventListener<E extends keyof MultiRoomMessageStoreEventHandlers>(
        event: E, 
        handler: MultiRoomMessageStoreEventHandlers[typeof event]
    ) {
        this._subscribers[event].add(handler);
        return () => this.removeEventListener(event, handler);
    }

    removeEventListener<E extends keyof MultiRoomMessageStoreEventHandlers>(
        event: E, 
        handler: MultiRoomMessageStoreEventHandlers[typeof event]
    ): void {
        this._subscribers[event].delete(handler);
    }

    fireEvent(
        event: keyof MultiRoomMessageStoreEventHandlers, 
        ...args: Parameters<MultiRoomMessageStoreEventHandlers[typeof event]>
    ): void {
        // @ts-expect-error spread args to handler
        this._subscribers[event].forEach(handler => handler(...args));
    }
    

    selectRoom(roomId: number): ChatMessage[] {
        this._selectedRoomId = roomId;
        const messages = this.getMessages();
        this.fireEvent("roomChanged", roomId);
        this.fireEvent("reset", ...[messages]);
        return messages;
    }

    getMessages(): ChatMessage[] {
        return this._getStore().messages;
    }

    append(...messages: ChatMessage[]): void {
        if (this._selectedRoomId === null) return;
        this._getStore().append(...messages);
        const removedFromStart = this._getStore().cutOffExcessStart();
        this.fireEvent("appended", messages);
        this.fireEvent("removedStart", removedFromStart);

    }

    prepend(...messages: ChatMessage[]): void {
        if (this._selectedRoomId === null) return;
        this._getStore().prepend(...messages);
        const removedFromEnd = this._getStore().cutOffExcessEnd();
        this.fireEvent("prepended", messages);
        this.fireEvent("removedEnd", removedFromEnd);
    }

    reset(messages: ChatMessage[]): void {
        if (this._selectedRoomId === null) return;
        this._getStore().reset(messages);
        this.fireEvent("reset", messages);
    }

    _getStore(): RoomMessageStore {
        if (this._selectedRoomId === null) {
            throw new Error("No store selected.");
        }
        if (!this._stores.has(this._selectedRoomId)) {
            this._createStore(this._selectedRoomId);
        }
        const storeData = this._stores.get(this._selectedRoomId)!;

        storeData.lastAccessed = Date.now();
        return storeData.store;
    }

    _createStore(roomId: number) {
        const store = new RoomMessageStore(this._messagesPerRoomLimit);
        this._stores.set(roomId, {store, lastAccessed: Date.now()});
        if (this._stores.size > this._roomLimit) {
            const leastRecentlyAccessed = Array
            .from(this._stores.entries())
            .sort((a, b) => a[1].lastAccessed - b[1].lastAccessed)[0];
            this._stores.delete(leastRecentlyAccessed[0]);
        }
    }
}