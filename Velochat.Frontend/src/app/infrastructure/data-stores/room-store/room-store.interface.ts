import type { Room } from "../../models";
import { type ISubscribable } from "../subscribable";





export interface IRoomStore extends ISubscribable<Room[]> {
    add(room: Room): void;
    remove(roomId: number): void;
}
