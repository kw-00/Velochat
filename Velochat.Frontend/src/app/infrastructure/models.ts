
export type User = {
    id: number;
    login: string;
}

export type Room = {
    id: number;
    name: string;
}

export type ChatMessage = {
    id: number;
    roomId: number;
    authorId: number;
    content: string;
}

export type Credentials = {
    login: string;
    password: string;
}
