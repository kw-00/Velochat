

export interface ISubscribableScalar<T> {
    get value(): T;

    subscribe(callback: (value: T) => void): () => void;

    set value(value: T);
}


export type SubscribableListEventHandlers<T> = {
    added: (added: T[]) => void;
    removed: (removed: T[]) => void;
    reset: (newElements: T[]) => void;
}

export interface ISubscribableList<T> {
    get value(): T[];

    addEventListener<E extends keyof SubscribableListEventHandlers<T>>(
        event: E, 
        handler: SubscribableListEventHandlers<T>[E]
    ): () => void;

    removeEventListener<E extends keyof SubscribableListEventHandlers<T>>(
        event: E, 
        handler: SubscribableListEventHandlers<T>[E]
    ): void;

    add(...element: T[]): void;
    remove(...element: T[]): void;
    reset(elements: T[]): void;
}
