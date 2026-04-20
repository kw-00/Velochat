import type { Identity } from "../../models";
import { AbstractSubscribableScalar } from "../subscribable";




export class IdentityStore extends AbstractSubscribableScalar<Identity | null> {

    private _identity: Identity | null = null;
    get(): Identity | null {
        return this._identity;
    }
    modify(modder: (current: Identity | null) => Identity | null): void {
        this._identity = modder(this._identity);
        this._notify();
    }

}