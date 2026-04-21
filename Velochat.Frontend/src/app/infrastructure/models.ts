
export type Invitation = {
    roomId: number;
    roomName: string;
    roomOwnerId: number;
    roomOwnerLogin: string;
}

export type User = {
    id: number;
    login: string;
}

export type Room = {
    id: number;
    name: string;
    ownerId: number;
}

export type ChatMessage = {
    id: number;
    roomId: number;
    authorId: number;
    content: string;
}
