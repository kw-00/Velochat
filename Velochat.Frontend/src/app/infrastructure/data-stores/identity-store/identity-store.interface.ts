import type { Identity } from "../../models";
import type { ISubscribableScalar } from "../subscribable";



// eslint-disable-next-line @typescript-eslint/no-empty-object-type
export interface IIdentityStore extends ISubscribableScalar<Identity | null> {}