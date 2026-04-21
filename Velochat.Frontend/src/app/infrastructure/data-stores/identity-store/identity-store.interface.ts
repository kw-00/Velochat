import type { User } from "../../models";
import type { ISubscribableScalar } from "../subscribable";



// eslint-disable-next-line @typescript-eslint/no-empty-object-type
export interface IUserStore extends ISubscribableScalar<User | null> {}