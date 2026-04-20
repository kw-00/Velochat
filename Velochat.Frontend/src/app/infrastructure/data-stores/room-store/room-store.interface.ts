import type { Room } from "../../models";
import { type ISubscribableScalar } from "../subscribable";





export interface IRoomStore extends ISubscribableScalar<Room[]> {
    add(room: Room): void;
    remove(roomId: number): void;
}
