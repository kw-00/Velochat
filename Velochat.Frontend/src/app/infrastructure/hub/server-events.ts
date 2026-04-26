
import type { ChatMessage, Room, User } from "../models";
import type { RealtimeConnection } from "./realtime-connection";



type ServerEventMap = {
    friendshipRequested: (user: User) => void;
    friendshipAccepted: (user: User) => void;
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
        // @ts-expect-error listener type
        this._connection.on(event, listener);
        return () => this.off(event, listener);
    }

    public off<K extends keyof ServerEventMap>(
        event: K, listener: ServerEventMap[K])
    : void {
        // @ts-expect-error listener type
        this._connection.off(event, listener);
    }
}