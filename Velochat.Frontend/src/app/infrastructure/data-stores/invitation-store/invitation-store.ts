import type { Invitation } from "../../models";
import { AbstractSubscribable } from "../subscribable";
import type { IInvitationStore } from "./invitation-store.interface";




export class InvitationStore 
    extends AbstractSubscribable<Invitation[]> 
    implements IInvitationStore 
{
    private _invitations: Invitation[] = [];

    get() {
        return this._invitations;
    }

    modify(modder: (current: Invitation[]) => Invitation[]): void {
        this._invitations = modder(this._invitations);
        this._notify();
    }

    add(invitation: Invitation) {
        this.modify((current) => [...current, invitation]);
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