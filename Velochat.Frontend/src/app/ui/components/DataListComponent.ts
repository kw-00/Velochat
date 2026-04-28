import type { DataComponentType } from "./DataComponent";




type DataListComponentType<
    TData, 
    TElement extends HTMLElement

> = TElement & {
    data: TData[];
    appendData(...data: TData[]): void;
    prependData(...data: TData[]): void;
    cutStart(count: number): TData[];
    cutEnd(count: number): TData[];
    reset(data: TData[]): void;
    sort(by?: ((el1: TData, el2: TData) => number)): void;
    map(mapper: (el: TData) => TData): void;
    removeData(predicate: (el: TData) => boolean): TData[];
};

export default function DataListComponent<
    TData, 
    TTag extends keyof HTMLElementTagNameMap,
    TItemElement extends DataComponentType<TData, HTMLElementTagNameMap[TTag]>
>(
    tag: TTag,
    elementFactory: (data: TData) => TItemElement,
) {
    const element = document
        .createElement(tag) as DataListComponentType<TData, HTMLElementTagNameMap[TTag]>;

    Object.defineProperty(element, "data", {
        get: function () {
            const children = Array.from(this.children);
            return children
                .filter(c => {
                    return (
                        c instanceof HTMLElement
                        && (c as DataComponentType<TData, TItemElement>).data !== undefined
                    );
                })
                .map(c => (c as DataComponentType<TData, TItemElement>).data);
        },
    });

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
    element.cutStart = function (count: number) {
        const children = Array.from(this.children) as TItemElement[];
        if (children.length > count) {
            const toRemove = children.slice(0, children.length - count);
            requestAnimationFrame(() => {
                toRemove.forEach(c => this.removeChild(c));
            });
            return toRemove.map(c => c.data);
        }
        return [];
    };
    element.cutEnd = function (count: number) {
        const children = Array.from(this.children) as TItemElement[];
        if (children.length > count) {
            const toRemove = children.slice(count);
            requestAnimationFrame(() => {
                toRemove.forEach(c => this.removeChild(c));
            });
            return toRemove.map(c => c.data);
        }
        return [];
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

    element.map = function (mapper: (el: TData) => TData) {
        const children = Array.from(this.children) as TItemElement[];
        const mapped = children.map(c => mapper(c.data));
        requestAnimationFrame(() => {
            this.replaceChildren(...mapped.map(elementFactory));
        });
        return mapped;
    };

    element.removeData = function (predicate: (el: TData) => boolean) {
        const children = Array.from(this.children) as TItemElement[];
        const toRemove = children.filter(c => predicate(c.data));
        requestAnimationFrame(() => {
            toRemove.forEach(c => this.removeChild(c));
        });
        return toRemove.map(c => c.data);
    };

    return element;
}


