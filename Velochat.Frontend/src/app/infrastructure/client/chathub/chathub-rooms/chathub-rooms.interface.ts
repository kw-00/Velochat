import type { Room } from "@/app/infrastructure/models";


export interface IChatHubRooms {
    createRoom(name: string): Promise<Room>;
    destroyRoom(roomId: number): Promise<void>;
    joinRoom(roomId: number): Promise<Room>;
    leaveRoom(roomId: number): Promise<void>;
}

