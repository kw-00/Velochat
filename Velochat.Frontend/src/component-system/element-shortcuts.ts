

/**
 * Creates an element.
 * @param type Element tag, eg. "div".
 * @param className The className of the element.
 * @param children Children of the element.
 * @returns An HTML element.
 */
export function $ce(
    type: keyof HTMLElementTagNameMap, 
    className: string, 
    ...children: (string | Node | null)[]
) : HTMLElement
{
    const element = document.createElement(type);
    element.className = className;
    const childrenNoNulls = children.filter(c => c !== null);
    element.append(...childrenNoNulls);
    return element;
}

/**
 * Creates an element without a className.
 * @param type Element tag, eg. "div".
 * @param children Children of the element.
 * @returns An HTML element.
 */
export function $cn(
    type: keyof HTMLElementTagNameMap,
    ...children: (string | Node | null)[]
)
{
    return $ce(type, "", ...children);
}