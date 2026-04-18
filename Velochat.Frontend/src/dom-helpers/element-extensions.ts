
declare global {
    interface HTMLElement {
        appendAndGet<T extends HTMLElement>(...children: (string | Node | null)[]) : T
    }
}

HTMLElement.prototype.appendAndGet = function appendAndGet<
    T extends HTMLElement
>(
    this: T, ...children: (string | Node | null)[]
): T 
{
    const childrenNoNulls = children.filter(c => c !== null);
    this.append(...childrenNoNulls);
    return this;
};
