import type { Invitation } from "../models";
import { AbstractSubscribable } from "./subscribable";




export class InvitationStore extends AbstractSubscribable<Invitation[]> {
    private _invitations: Invitation[] = [];

    get() {
        return this._invitations;
    }

    modify(modder: (current: Invitation[]) => Invitation[]): void {
        this._invitations = modder(this._invitations);
        this._notify();
    }

}

const invitationStore = new InvitationStore();

invitationStore.modify(() => new Array(100).fill({
    roomId: 0,
    roomName: "Some Room",
    roomOwnerId: 1,
    roomOwnerLogin: "admin"
}));
export default invitationStore;