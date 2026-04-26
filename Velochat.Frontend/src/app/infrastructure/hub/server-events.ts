
import type { ChatMessage, Room, User } from "../models";
import type { RealtimeConnection } from "./realtime-connection";



type ServerEventMap = {
    friendshipRequested: (requester: User) => void;
    friendshipAccepted: (accepter: User) => void;
    unfriended: (formerFriend: User) => void;
    addedToRoom: (room: Room) => void;

    message: (message: ChatMessage) => void;
    userJoined: (user: User) => void;
    userLeft: (user: User) => void;
}

export class ServerEvents {
    private _connection: RealtimeConnection;

    constructor(connection: RealtimeConnection) {
        this._connection = connection;
    }

    public on<K extends keyof ServerEventMap>(
        event: K, listener: ServerEventMap[K]
    ): () => void {
        const eventCapitalized = `${event[0].toUpperCase()}${event.slice(1)}`;
        // @ts-expect-error listener type
        this._connection.on(eventCapitalized, listener);
        return () => this.off(event, listener);
    }

    public off<K extends keyof ServerEventMap>(
        event: K, listener: ServerEventMap[K])
    : void {
        // @ts-expect-error listener type
        this._connection.off(event, listener);
    }
}