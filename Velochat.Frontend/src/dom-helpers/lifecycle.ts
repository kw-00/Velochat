

const WITH_CALLABACKS_TAG = "with-callbacks";

type Callbacks = {
    connectedCallback: () => void,
    disconnectedCallback: () => void,
    connectedMovedCallback: () => void,
    adoptedCallback: () => void,
    attributeChangedCallback: (
        name: string, 
        oldValue: string, 
        newValue: string
    ) => void,
}

class WithCallbacksElement extends HTMLElement {
    private _callbacks: Callbacks = {
        connectedCallback: () => {},
        disconnectedCallback: () => {},
        connectedMovedCallback: () => {},
        adoptedCallback: () => {},
        attributeChangedCallback: () => {},
    };


    setUpCallbacks(callbacks: Partial<Callbacks>) {
        Object.assign(this._callbacks, callbacks);
    }

    setCallback<K extends keyof Callbacks>(callbackName: K, callback: Callbacks[K]) {
        this._callbacks[callbackName] = callback;
    }

    connectedCallback() {
        this._callbacks.connectedCallback();
    }
    disconnectedCallback() {
        this._callbacks.disconnectedCallback();
    }
    connectedMovedCallback() {
        this._callbacks.connectedMovedCallback();
    }
    adoptedCallback() {
        this._callbacks.adoptedCallback();
    }
    attributeChangedCallback(name: string, oldValue: string, newValue: string) {
        this._callbacks.attributeChangedCallback(name, oldValue, newValue);
    }

}

customElements.define(WITH_CALLABACKS_TAG, WithCallbacksElement);

export function createElementWithCallbacks(callbacks?: Partial<Callbacks>) {
    const element = document.createElement(WITH_CALLABACKS_TAG) as WithCallbacksElement;
    if (callbacks !== undefined) element.setUpCallbacks(callbacks);
    return element;
}