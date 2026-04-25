import * as SignalR from "@microsoft/signalr";
import type { ChatMessage, Room, User } from "../models";



type ServerEventMap = {
    friendshipRequested: (user: User) => void;
    friendshipAccepted: (user: User) => void;
    addedToRoom: (room: Room) => void;

    message: (message: ChatMessage) => void;
    userJoined: (user: User) => void;
    userLeft: (user: User) => void;
}

export class ServerEvents {
    private _connection: SignalR.HubConnection;

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    public on<K extends keyof ServerEventMap>(
        event: K, listener: ServerEventMap[K]
    ): () => void {
        this._connection.on(event, listener);
        return () => this.off(event, listener);
    }

    public off<K extends keyof ServerEventMap>(
        event: K, listener: ServerEventMap[K])
    : void {
        this._connection.off(event, listener);
    }
}