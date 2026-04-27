import { MessagingHubClient, type FeedUpdate } from "./hub/messaging";
import type { RealtimeConnection } from "./hub/realtime-connection";
import type { ServerEvents } from "./hub/server-events";
import type { ChatMessage } from "./models";
import { LimitedList } from "./observable/limited-list";


type MessageManagerEventMap = {
    appended: (appended: Array<ChatMessage>, cutStart: Array<ChatMessage>) => void
    prepended: (prepended: Array<ChatMessage>, cutEnd: Array<ChatMessage>) => void;
    overwritten: (before: Array<ChatMessage>, after: Array<ChatMessage>) => void
}

type MessageManagerListenerContainer = {
    [key in keyof MessageManagerEventMap]: Set<MessageManagerEventMap[key]> 
}

export class MessageManager {
    private _selection: number | null = null;

    private _messagingClient: MessagingHubClient;

    private _rooms = new Map<number, Array<ChatMessage>>();
    private _currentRoomMessages = new LimitedList<ChatMessage>(300);
    private _listeners: MessageManagerListenerContainer = {
        "appended": new Set(),
        "prepended": new Set(),
        "overwritten": new Set()
    };

    private _disposeListeners = new Set<() => void>();

    constructor(
        realtimeConnection: RealtimeConnection,
        serverEvents: ServerEvents,
        messagingHubClient: MessagingHubClient
    ) {
        this._messagingClient = messagingHubClient;
        
        for (const key in this._listeners) {
            // @ts-expect-error key type widened to string
            this._currentRoomMessages.on(key, (...args) => this._fire(key, ...args));
        };

        this._onDispose(
            serverEvents.on("message", m => {
                if (m.roomId !== this._selection) return;
                this._currentRoomMessages.append(m);
            })
        );
        realtimeConnection.onconnected(() => {
            if (this._selection === null) return;
            this.subscribeFeedAsync();
        });
    }

    on<K extends keyof MessageManagerEventMap>(
        event: K, listener: MessageManagerEventMap[K]
    ): () => void {
        this._listeners[event].add(listener);
        return () => this._listeners[event].delete(listener);
    }

    async goOlderAsync() {
        const selection = this._selection;
        if (selection === null) {
            throw new Error("Cannot go to older messages. No room selected.");
        }
        const cache = this._getOrInitCache(selection);
        if (cache.length === 0) throw new Error(
            "Cannot go to older messages,"
            + " as there are no messages at all."
        );

        const oldestCachedMessageId = cache[0].id;
        const goOlderResult = await this._messagingClient.goOlderAsync(oldestCachedMessageId);
        if (!goOlderResult.success) throw new Error(goOlderResult.message);
        const olderMessages = goOlderResult.data.messages;
        this._currentRoomMessages.overwrite(olderMessages, "end");
    }

    async goNewerAsync() {
        const selection = this._selection;
        if (selection === null) {
            throw new Error("Cannot go to newer messages. No room selected.");
        }
        const cache = this._getOrInitCache(selection);
        if (cache.length === 0) throw new Error(
            "Cannot go to newer messages,"
            + " as there are no messages at all."
        );

        const newestCachedMessageId = cache[cache.length - 1].id;
        const goNewerResult = await this._messagingClient.goNewerAsync(newestCachedMessageId);
        if (!goNewerResult.success) throw new Error(goNewerResult.message);
        const newerMessages = goNewerResult.data.messages;
        this._currentRoomMessages.overwrite(newerMessages, "start");
    }

    async subscribeFeedAsync() {
        const newestCachedMessageId = this._getNewestCachedMessageId();
        const subscribeFeedResult = await this._messagingClient
            .subscribeFeedAsync(newestCachedMessageId);

        if (!subscribeFeedResult.success) 
            throw new Error(subscribeFeedResult.message);

        this._reconcileCache(subscribeFeedResult.data);
    }

    async unsubscribeFeedAsync() {
        const unsubscribeFeedResult = await this._messagingClient.unsubscribeFeedAsync();
        if (!unsubscribeFeedResult.success) 
            throw new Error(unsubscribeFeedResult.message);
    }

    async switchAsync(roomId: number) {
        this._cacheMessages();
        this._selection = roomId;

        const newestCachedMessageId = this._getNewestCachedMessageId();
        const switchResult = await this._messagingClient
            .switchFocusAsync(roomId, newestCachedMessageId);

        if (!switchResult.success) throw new Error(switchResult.message);
        this._reconcileCache(switchResult.data);
    }

    async zoneOutAsync() {
        await this._messagingClient.zoneOutAsync();
        this._cacheMessages();
        this._selection = null;
        this._currentRoomMessages.overwrite([], "start");
    }

    removeRoomFromCache(roomId: number, switchTo: number | null) {
        this._rooms.delete(roomId);
        if (this._rooms.size === 0 || switchTo === null) {
            this.zoneOutAsync();
        } else {
            this.switchAsync(switchTo);
        }
        if (this._selection === roomId) {
            this._selection = null;
            this._currentRoomMessages.overwrite([], "start");
        }
    }

    async [Symbol.asyncDispose]() {
        this._disposeListeners.forEach(callback => callback());
    }

    private _onDispose(callback: () => void) {
        this._disposeListeners.add(callback);
    }

    private _cacheMessages() {
        if (this._selection === null) return;
        if (this._currentRoomMessages.data.length === 0) {
            this._rooms.delete(this._selection);
        }
        this._rooms.set(this._selection, this._currentRoomMessages.data);
    }

    private _getNewestCachedMessageId() {
        const selection = this._selection;
        if (selection === null) throw new Error(
            "Cannot get newest message. No room selected."
        );
        const cached = this._getOrInitCache(selection);
        return cached.length > 0 ? cached[cached.length - 1].id : null;
    }

    private _reconcileCache(feedUpdate: FeedUpdate) {
        const selection = this._selection;
        if (selection === null) throw new Error(
            "Cannot reconcile cache. No room selected."
        );
        const cached = this._getOrInitCache(selection);
        let reconciledMessageData;
        if (feedUpdate.isContinuity) {
            reconciledMessageData = [...cached, ...feedUpdate.messages];
        } else {
            reconciledMessageData = feedUpdate.messages;
        }
        this._currentRoomMessages.overwrite(reconciledMessageData, "start");
        this._rooms.set(selection, reconciledMessageData);
    }

    private _getOrInitCache(roomId: number) {
        let cached = this._rooms.get(roomId);
        if (cached === undefined) {
            cached = [];
            this._rooms.set(roomId, cached);
        }
        return cached;
    }

    private _fire<K extends keyof MessageManagerEventMap>(
        event: K, ...args: Parameters<MessageManagerEventMap[K]>
    ): void {
        // @ts-expect-error spread args into listener
        this._listeners[event].forEach(listener => listener(...args));
    }


}