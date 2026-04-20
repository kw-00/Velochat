import type { Invitation } from "../../models";
import { AbstractSubscribableScalar } from "../subscribable";
import type { IInvitationStore } from "./invitation-store.interface";




export class InvitationStore 
    extends AbstractSubscribableScalar<Invitation[]> 
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
