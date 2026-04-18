import type { Invitation, Room } from "@/app/infrastructure/models";


export type InitialChatInformation = {
    rooms: Room[],
    invitations: Invitation[]
}

export interface IChatHubInit {
    getInitialChatInformation(): Promise<InitialChatInformation>
}

