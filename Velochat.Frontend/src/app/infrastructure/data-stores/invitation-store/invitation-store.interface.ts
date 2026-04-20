import type { Invitation } from "../../models";
import type { ISubscribableScalar } from "../subscribable";




export interface IInvitationStore extends ISubscribableScalar<Invitation[]> {
    add(invitation: Invitation): void;
}