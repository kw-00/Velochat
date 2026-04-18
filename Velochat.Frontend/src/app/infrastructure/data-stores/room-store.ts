import type { Room } from "../models";
import { AbstractSubscribable } from "./subscribable";





export class RoomStore extends AbstractSubscribable<Room[]> {
    private _rooms: Room[] = [];

    get() {
        return this._rooms;
    }

    modify(modder: (current: Room[]) => Room[]) {
        this._rooms = modder(this._rooms);
        this._notify();
    }
}

const globalRoomStore = new RoomStore();

globalRoomStore.modify(() => new Array<Room>(100).fill({id: 0, name: "The Void", ownerId: 1}));

export default globalRoomStore;