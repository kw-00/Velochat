

export class Observable<T> {
    private _value: T;
    private _listeners = new Set<(before: T, after: T) => void>();

    get value() {
        return this._value;
    }

    set value(value: T) {
        this._value = value;
        this._notify();
    }

    constructor(initialValue: T) { 
        this._value = initialValue;
    }

    subscribe(listener: (before: T, after: T) => void): () => void {
        this._listeners.add(listener);
        return () => this._listeners.delete(listener);
    }

    private _notify() {
        this._listeners.forEach(listener => listener(this._value, this._value));
    }
}