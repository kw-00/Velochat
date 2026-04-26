import type { RealtimeConnection } from "./hub/realtime-connection";
import type { RoomsHubClient } from "./hub/rooms";
import type { ServerEvents } from "./hub/server-events";
import type { Room } from "./models";
import { LimitedList } from "./observable/limited-list";





type RoomManagerEventMap = {
    "added": (added: Room[]) => void;
    "removed": (removed: Room[]) => void;
    "overwritten": (before: Room[], after: Room[]) => void
}

type RoomManagerListenerMap = {
    [key in keyof RoomManagerEventMap]: Set<RoomManagerEventMap[key]>;
}

export class RoomManager {
    private _chatrooms = new LimitedList<Room>(null);
    private _roomsHubClient: RoomsHubClient;
    private _listeners: RoomManagerListenerMap = {
        "added": new Set(),
        "removed": new Set(),
        "overwritten": new Set()
    };

    private _disposedListeners = new Set<() => void>();

    get chatrooms() {
        return this._chatrooms.data;
    }

    constructor(
        realtimeConnection: RealtimeConnection,
        serverEvents: ServerEvents,
        roomsHubClient: RoomsHubClient
    ) {
        this._roomsHubClient = roomsHubClient;

        this._chatrooms.on("appended", r => this._fire("added", r));
        this._chatrooms.on("removed", r => this._fire("removed", r));
        this._chatrooms.on("overwritten", (before, after) => this._fire("overwritten", before, after));

        this._onDisposed(
            serverEvents.on("addedToRoom", r => this._chatrooms.append(r))
        );
        realtimeConnection.onconnected(() => this.refreshRoomsAsync());
    }

    async refreshRoomsAsync() {
        const rooms = await this._roomsHubClient.getRoomsAsync();
        if (!rooms.success) throw new Error(rooms.message);
        this._chatrooms.overwrite(rooms.data, "start");
    }

    async createRoomAsync(name: string) {
        const creationResult =  await this._roomsHubClient.createRoomAsync(name);
        if (!creationResult.success) throw new Error(creationResult.message);
        this._chatrooms.append(creationResult.data);
    }

    async leaveRoomAsync(roomId: number) {
        const leaveResult = await this._roomsHubClient.leaveRoomAsync(roomId);
        if (!leaveResult.success) throw new Error(leaveResult.message);
        this._chatrooms.remove(r => r.id === roomId);
    }


    on<K extends keyof RoomManagerEventMap>(
        event: K, listener: RoomManagerEventMap[K]
    ): () => void {
        this._listeners[event].add(listener);
        return () => this._listeners[event].delete(listener);
    }

    async [Symbol.asyncDispose]() {
        this._disposedListeners.forEach(listener => listener());
    }

    _onDisposed(listener: () => void) {
        this._disposedListeners.add(listener);
    }

    _fire<K extends keyof RoomManagerEventMap>(
        event: K, ...args: Parameters<RoomManagerEventMap[K]>
    ): void {
        // @ts-expect-error spread args into listener
        this._listeners[event].forEach(listener => listener(...args));
    }
}