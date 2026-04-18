import type { Invitation } from "../../models";
import type { ISubscribable } from "../subscribable";




export interface IInvitationStore extends ISubscribable<Invitation[]> {
    add(invitation: Invitation): void;
}