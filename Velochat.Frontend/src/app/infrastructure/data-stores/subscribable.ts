

export interface ISubscribable<T> {
    get(): T;

    subscribe(callback: (value: T) => void): () => void;
}

export abstract class AbstractSubscribable<T> implements ISubscribable<T> {
    private _subscriptions: Set<(value: T) => void> = new Set();

    overwrite(value: T): void {
        this.modify(() => value);
    }
    
    subscribe(callback: (value: T) => void): () => void {
        this._subscriptions.add(callback);
        return () => this._subscriptions.delete(callback);
    }


    _notify() {
        const value = this.get();
        this._subscriptions.forEach(callback => callback(value));
    }

    abstract get(): T;
    abstract modify(modder: (current: T) => T): void;
}

