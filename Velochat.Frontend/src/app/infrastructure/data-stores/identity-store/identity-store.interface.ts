import type { Identity } from "../../models";
import type { ISubscribable } from "../subscribable";



// eslint-disable-next-line @typescript-eslint/no-empty-object-type
export interface IIdentityStore extends ISubscribable<Identity | null> {}