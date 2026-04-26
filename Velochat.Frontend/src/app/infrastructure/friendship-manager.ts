




import type { FriendshipHubClient } from "./hub/friendship";
import type { RealtimeConnection } from "./hub/realtime-connection";
import type { ServerEvents } from "./hub/server-events";
import type { User } from "./models";
import { LimitedList } from "./observable/limited-list";





type FriendshipManagerEventMap = {
    "friendshipRequested": (requesters: User[]) => void;
    "requestDeclined": (usersDeclined: User[]) => void;
    "requestsRefreshed": (before: User[], after: User[]) => void;
    "friendshipAccepted": (acceptedBy: User[]) => void;
    "friendshipVoided": (noLongerFriends: User[]) => void;
    "friendsRefreshed": (before: User[], after: User[]) => void;
}

type FriendshipManagerListenerMap = {
    [key in keyof FriendshipManagerEventMap]: Set<FriendshipManagerEventMap[key]>;
}

export class FriendshipManager {
    private _pendingFriendshipRequesters = new LimitedList<User>(null);
    private _friends = new LimitedList<User>(null);
    private _friendshipHubClient: FriendshipHubClient;
    private _listeners: FriendshipManagerListenerMap = {
        "friendshipRequested": new Set(),
        "requestDeclined": new Set(),
        "requestsRefreshed": new Set(),
        "friendshipAccepted": new Set(),
        "friendshipVoided": new Set(),
        "friendsRefreshed": new Set()
    };

    private _disposedListeners = new Set<() => void>();

    get chatrooms() {
        return this._friends.data;
    }

    constructor(
        realtimeConnection: RealtimeConnection,
        serverEvents: ServerEvents,
        friendshipHubClient: FriendshipHubClient
    ) {
        this._friendshipHubClient = friendshipHubClient;

        
        this._pendingFriendshipRequesters.on("appended", u => {
            this._fire("friendshipRequested", u);
        });
        this._pendingFriendshipRequesters.on("removed", u => {
            this._fire("requestDeclined", u);
        });
        this._pendingFriendshipRequesters.on("overwritten", (before, after) => {
            this._fire("friendsRefreshed", before, after);
        });
        this._onDisposed(
            serverEvents.on("friendshipRequested", u => {
                this._pendingFriendshipRequesters.append(u);
            })
        );
        realtimeConnection.onconnected(() => this.refreshRequestsAsync());

        
        this._friends.on("appended", u => this._fire("friendshipAccepted", u));
        this._friends.on("removed", u => this._fire("friendshipVoided", u));
        this._friends.on("overwritten", (before, after) => {
            this._fire("friendsRefreshed", before, after);
        });
        this._onDisposed(
            serverEvents.on("friendshipAccepted", u => this._friends.append(u))
        );
        realtimeConnection.onconnected(() => this.refreshFriendshipsAsync());

    }

    async requestFriendship(userId: number) {
        const result = await this._friendshipHubClient.requestAsync(userId);
        if (!result.success) throw new Error(result.message);
    }
    
    async declineRequest(userId: number) {
        const result = await this._friendshipHubClient.declineAsync(userId);
        if (!result.success) throw new Error(result.message);
        this._pendingFriendshipRequesters.remove(u => u.id === userId);
    }

    async refreshRequestsAsync() {
        const requests = await this._friendshipHubClient.getRequestsAsync();
        if (!requests.success) throw new Error(requests.message);
        this._pendingFriendshipRequesters.overwrite(requests.data, "start");
    }
    
    async acceptRequest(userId: number) {
        const result = await this._friendshipHubClient.acceptAsync(userId);
        if (!result.success) throw new Error(result.message);
        this._pendingFriendshipRequesters.remove(u => u.id === userId);
        this._friends.append(result.data);
    }

    async removeFriendship(userId: number) {
        const result = await this._friendshipHubClient.removeFriendAsync(userId);
        if (!result.success) throw new Error(result.message);
        this._friends.remove(u => u.id === userId);
    }

    async refreshFriendshipsAsync() {
        const friends = await this._friendshipHubClient.getFriendsAsync();
        if (!friends.success) throw new Error(friends.message);
        this._friends.overwrite(friends.data, "start");
    }



    on<K extends keyof FriendshipManagerEventMap>(
        event: K, listener: FriendshipManagerEventMap[K]
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

    _fire<K extends keyof FriendshipManagerEventMap>(
        event: K, ...args: Parameters<FriendshipManagerEventMap[K]>
    ): void {
        // @ts-expect-error spread args into listener
        this._listeners[event].forEach(listener => listener(...args));
    }
}