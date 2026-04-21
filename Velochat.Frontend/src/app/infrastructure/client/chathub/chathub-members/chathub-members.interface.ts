


export interface IChatHubMembers {
    invite(roomId: number, userId: number): Promise<void>;
    revokeInvitation(roomId: number, userId: number): Promise<void>;
    kickMember(roomId: number, userId: number): Promise<void>;
}

