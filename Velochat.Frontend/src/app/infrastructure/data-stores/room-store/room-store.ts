import type { Room } from "../../models";
import { AbstractSubscribableScalar } from "../subscribable";
import type { IRoomStore } from "./room-store.interface";





export class RoomStore 
    extends AbstractSubscribableScalar<Room[]> 
    implements IRoomStore
{
    private _rooms: Room[] = [];

    get() {
        return this._rooms;
    }

    modify(modder: (current: Room[]) => Room[]) {
        this._rooms = modder(this._rooms);
        this._notify();
    }

    add(room: Room) {
        this.modify((current) => [...current, room]);
    }

    remove(roomId: number) {
        this.modify((current) => current.filter(room => room.id !== roomId));
    }
}
