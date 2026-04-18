import type { Identity } from "../../models";
import { AbstractSubscribable } from "../subscribable";




export class IdentityStore extends AbstractSubscribable<Identity | null> {

    private _identity: Identity | null = null;
    get(): Identity | null {
        return this._identity;
    }
    modify(modder: (current: Identity | null) => Identity | null): void {
        this._identity = modder(this._identity);
        this._notify();
    }

}