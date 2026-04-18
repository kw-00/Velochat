


export interface IChatHubMembers {
    invite(roomId: number, identityId: number): Promise<void>;
    revokeInvitation(roomId: number, identityId: number): Promise<void>;
    kickMember(roomId: number, identityId: number): Promise<void>;
}

