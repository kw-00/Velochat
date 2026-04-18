import type { ChatMessage } from "@/app/infrastructure/models";


export interface IChatHubMessages {
    sendMessage(roomId: number, content: string): Promise<ChatMessage>;
    getMessagesBefore(roomId: number, before: number): Promise<ChatMessage[]>;
    getMessagesAfter(roomId: number, after: number): Promise<ChatMessage[]>;
    getRecentMessages(roomId: number): Promise<ChatMessage[]>;
}

