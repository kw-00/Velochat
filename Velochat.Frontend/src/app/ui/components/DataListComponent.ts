import type { DataComponentType } from "./DataComponent";




type DataListComponentType<
    TData, 
    TElement extends HTMLElement

> = TElement & {
    capacity: number;
    appendData(...data: TData[]): void;
    prependData(...data: TData[]): void;
    cutStart(): void;
    cutEnd(): void;
    reset(data: TData[]): void;
    sort(by?: ((el1: TData, el2: TData) => number)): void;
    removeData(predicate: (el: TData) => boolean): void;
};

export default function DataListComponent<
    TData, 
    TTag extends keyof HTMLElementTagNameMap,
    TItemElement extends DataComponentType<TData, HTMLElementTagNameMap[TTag]>
>(
    tag: TTag,
    elementFactory: (data: TData) => TItemElement,
    capacity: number
) {
    const element = document
        .createElement(tag) as DataListComponentType<TData, HTMLElementTagNameMap[TTag]>;

    element.capacity = capacity;

    element.appendData = function (...data: TData[]) {
        const frag = document.createDocumentFragment();
        data.forEach(d => frag.append(elementFactory(d)));
        this.append(frag);
    };
    element.prependData = function (...data: TData[]) {
        const frag = document.createDocumentFragment();
        data.forEach(d => frag.append(elementFactory(d)));
        this.prepend(frag);
    };
    element.cutStart = function () : void {
        const children = Array.from(this.children) as TItemElement[];
        if (children.length > this.capacity) {
            const toRemove = children.slice(0, children.length - this.capacity);
            requestAnimationFrame(() => {
                toRemove.forEach(c => this.removeChild(c));
            });
        }
    };
    element.cutEnd = function () : void {
        const children = Array.from(this.children) as TItemElement[];
        if (children.length > this.capacity) {
            const toRemove = children.slice(this.capacity);
            requestAnimationFrame(() => {
                toRemove.forEach(c => this.removeChild(c));
            });
        }
    };

    element.reset = function (data: TData[]) {
        requestAnimationFrame(() => {
            this.replaceChildren(...data.map(elementFactory));
        });
    };

    element.sort = function (by?: (el1: TData, el2: TData) => number) {
        const children = Array.from(this.children) as TItemElement[];
        if (by === undefined) children.sort();
        else children.sort((a, b) => by(a.data, b.data));
        requestAnimationFrame(() => {
            this.replaceChildren(...children);
        });
    };

    element.removeData = function (predicate: (el: TData) => boolean) {
        const children = Array.from(this.children) as TItemElement[];
        const toRemove = children.filter(c => predicate(c.data));
        requestAnimationFrame(() => {
            toRemove.forEach(c => this.removeChild(c));
        });
    };

    return element;
}


