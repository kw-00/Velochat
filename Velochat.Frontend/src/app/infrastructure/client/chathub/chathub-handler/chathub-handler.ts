import * as SignalR from "@microsoft/signalr";
import type { ChatHubEventHandlerMap, IChatHubHandler } from "./chathub-handler.inteface";
import type { ChatMessage, Invitation } from "@/app/infrastructure/models";

export default class ChatHubHandler implements IChatHubHandler {
    private _connection: SignalR.HubConnection;
    private _listeners: {
        [K in keyof ChatHubEventHandlerMap]: Set<ChatHubEventHandlerMap[K]>
    } = {
        roomClosed: new Set(),
        invited: new Set(),
        kicked: new Set(),
        messageReceived: new Set()
    };

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
        this._connection.on("RoomClosed", this._roomClosed);
        this._connection.on("Invited", this._invited);
        this._connection.on("Kicked", this._kicked);
        this._connection.on("MessageReceived", this._messageReceived);
    }

    addEventListener<T extends keyof ChatHubEventHandlerMap>(
        event: T, handler: ChatHubEventHandlerMap[T]
    ): void {
        this._listeners[event].add(handler);
    }
    removeEventListener<T extends keyof ChatHubEventHandlerMap>(
        event: T, handler: ChatHubEventHandlerMap[T]
    ): void {
        this._listeners[event].delete(handler);
    }

    _fireEvent<T extends keyof ChatHubEventHandlerMap>(
        event: T,
        ...args: Parameters<ChatHubEventHandlerMap[T]>
    ): void {
        for (const handler of this._listeners[event]) {
            // @ts-expect-error spread args to handler
            handler(...args);
        }
    }


    _roomClosed(roomId: number) {
        this._fireEvent("roomClosed", roomId);
    }

    _invited(invitation: Invitation) {
        this._fireEvent("invited", invitation);
    }

    _kicked(roomId: number) {
        this._fireEvent("kicked", roomId);
    }

    _messageReceived(message: ChatMessage) {
        this._fireEvent("messageReceived", message);
    }
}