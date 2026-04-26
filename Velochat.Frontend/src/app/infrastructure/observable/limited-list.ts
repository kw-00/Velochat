

type LimitedListEventMap<T> = {
    "appended": (appended: T[], cutStart: T[]) => void;
    "prepended": (prepended: T[], cutEnd: T[]) => void;
    "overwritten": (before: T[], after: T[]) => void;
}

type LimitedListListenerContainer<T> = {
    [key in keyof LimitedListEventMap<T>]: Set<LimitedListEventMap<T>[key]>;
}


export class LimitedList<T> {
    capacity: number | null;
    private _data = new Array<T>();
    private _listeners: LimitedListListenerContainer<T> = {
        "appended": new Set(),
        "prepended": new Set(),
        "overwritten": new Set()
    };

    get data() {
        return [...this._data];
    }

    constructor(capacity?: number | null, ...elements: T[]) {
        this._data.push(...elements); 
        this.capacity = capacity ?? null; 
    }

    on<K extends keyof LimitedListEventMap<T>>(
        event: K, listener: LimitedListEventMap<T>[K]
    ): () => void {
        this._listeners[event].add(listener);
        return () => this._listeners[event].delete(listener);
    }
    
    append(...elements: T[]): void {
        this._data.push(...elements);
        const cut = this._cutStart();
        this._fire("appended", elements, cut);
    }

    prepend(...elements: T[]): void {
        this._data.unshift(...elements);
        const cut = this._cutEnd();
        this._fire("prepended", elements, cut);
    }
    
    overwrite(newElements: T[], cutFrom: "start" | "end"): void {
        const oldElements = this._data.splice(0);
        this._data.push(...newElements);
        if (cutFrom === "start") this._cutStart();
        else this._cutEnd();
        this._fire("overwritten", oldElements, [...this._data]);
    }

    private _cutStart(): T[] {
        let cut: T[];
        if (this.capacity !== null && this._data.length > this.capacity) {
            cut = this._data.splice(this._data.length - this.capacity);
        } else {
            cut = [];
        }
        return cut;
    }

    private _cutEnd(): T[] {
        let cut: T[];
        if (this.capacity !== null && this._data.length > this.capacity) {
            cut = this._data.splice(0, this._data.length - this.capacity);
        } else {
            cut = [];
        }
        return cut;
    }

    private _fire<K extends keyof LimitedListEventMap<T>>(
        event: K, ...args: Parameters<LimitedListEventMap<T>[K]>
    ): void {
        // @ts-expect-error spread args into listener
        this._listeners[event].forEach(listener => listener(...args));
    }
}