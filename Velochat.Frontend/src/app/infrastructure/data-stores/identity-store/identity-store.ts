import type { User } from "../../models";
import { AbstractSubscribableScalar } from "../subscribable";




export class UserStore extends AbstractSubscribableScalar<User | null> {

    private _user: User | null = null;
    get(): User | null {
        return this._user;
    }
    modify(modder: (current: User | null) => User | null): void {
        this._user = modder(this._user);
        this._notify();
    }

}