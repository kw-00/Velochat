import type { ISubscribableList, ISubscribableScalar, SubscribableListEventHandlers } from "./subscribable.interface";



export class SubscribableScalar<T> implements ISubscribableScalar<T> {
    private _subscriptions: Set<(value: T) => void> = new Set();
    private _value: T;

    get value(): T {
        return this._value;
    }

    constructor(value: T) {
        this._value = value;
    }

    set value(value: T) {
        this._value = value;
        this._notify();
    }
    
    subscribe(callback: (value: T) => void): () => void {
        this._subscriptions.add(callback);
        return () => this._subscriptions.delete(callback);
    }


    _notify() {
        const value = this._value;
        this._subscriptions.forEach(callback => callback(value));
    }
}


export class SubscribableList<T> implements ISubscribableList<T> {
    private _subscriptions = {
        added: new Set<(added: T[]) => void>(),
        removed: new Set<(removed: T[]) => void>(),
        reset: new Set<(newElements: T[]) => void>()
    };
    private _value: T[] = [];

    get value(): T[] {
        return this._value;
    }

    addEventListener<E extends keyof SubscribableListEventHandlers<T>>(
        event: E, 
        handler: SubscribableListEventHandlers<T>[E]
    ): () => void {
        this._subscriptions[event].add(handler);
        return () => this.removeEventListener(event, handler);
    }

    removeEventListener<E extends keyof SubscribableListEventHandlers<T>>(
        event: E, 
        handler: SubscribableListEventHandlers<T>[E]
    ): void {
        this._subscriptions[event].delete(handler);
    }

    fireEvent(
        event: keyof SubscribableListEventHandlers<T>, 
        ...args: Parameters<SubscribableListEventHandlers<T>[keyof SubscribableListEventHandlers<T>]>
    ): void {
        // @ts-expect-error spread args to handler
        this._subscriptions[event].forEach(handler => handler(...args));
    }

    add(...element: T[]): void {
        this._value.push(...element);
        this.fireEvent("added", element);
    }

    remove(...element: T[]): void {
        const removed = this._value.filter(e => element.includes(e)).map(e => e);
        this._value = this._value.filter(e => !element.includes(e));
        this.fireEvent("removed", removed);
    }

    reset(elements: T[]): void {
        this._value = elements;
        this.fireEvent("reset", elements);
    }
}

