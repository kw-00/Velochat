import type { ChatMessage } from "../../models";
import type { IGlobalMessageStore } from "./message-store.interface";


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
        this._cutOffExcessStart();
    }

    prepend(...messages: ChatMessage[]): void {
        this._messages = messages.concat(this._messages);
        this._cutOffExcessEnd();
    }

    reset(messages: ChatMessage[]): void {
        this._messages = messages;
        this._cutOffExcessStart();
    }

    private _cutOffExcessStart() {
        if (this._messages.length > this._capacity) {
            this._messages = this._messages.slice(this._messages.length - this._capacity);
        }
    }

    private _cutOffExcessEnd() {
        if (this._messages.length > this._capacity) {
            this._messages = this._messages.slice(0, this._capacity);
        }
    }
}

export class GlobalMessageStore implements IGlobalMessageStore {
    _stores: Map<number, {store: RoomMessageStore, lastAccessed: number}> = new Map();
    _messagesPerRoomLimit: number;
    _roomLimit: number;

    _selectedRoomId: number | null = null;

    _messagesChangedSubscribers: Set<(messages: ChatMessage[]) => void> = new Set();
    _roomChangedSubscribers: Set<(roomId: number) => void> = new Set();

    get selectedRoomId(): number | null {
        return this._selectedRoomId;
    }

    constructor(messagesPerRoomLimit: number, roomLimit: number) {
        this._messagesPerRoomLimit = messagesPerRoomLimit;
        this._roomLimit = roomLimit;
    }

    subscribeMessagesChanged(callback: (messages: ChatMessage[]) => void): () => void {
        this._messagesChangedSubscribers.add(callback);
        return () => this._messagesChangedSubscribers.delete(callback);
    }

    subscribeRoomChanged(callback: (roomId: number) => void): () => void {
        this._roomChangedSubscribers.add(callback);
        return () => this._roomChangedSubscribers.delete(callback);
    }

    selectRoom(roomId: number): ChatMessage[] {
        this._selectedRoomId = roomId;
        const messages = this.getMessages();
        this._fireMessagesChanged();
        this._fireRoomChanged();
        return messages;
    }

    getMessages(): ChatMessage[] {
        return this._getStore().messages;
    }

    append(...messages: ChatMessage[]): void {
        if (this._selectedRoomId === null) return;
        this._getStore().append(...messages);
        this._fireMessagesChanged();
    }

    prepend(...messages: ChatMessage[]): void {
        if (this._selectedRoomId === null) return;
        this._getStore().prepend(...messages);
        this._fireMessagesChanged();
    }

    reset(messages: ChatMessage[]): void {
        if (this._selectedRoomId === null) return;
        this._getStore().reset(messages);
        this._fireMessagesChanged();
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

    _fireMessagesChanged() {
        const messages = this.getMessages();
        this._messagesChangedSubscribers.forEach(callback => callback(messages));
    }

    _fireRoomChanged() {
        this._roomChangedSubscribers.forEach(callback => callback(this._selectedRoomId!));
    }
}