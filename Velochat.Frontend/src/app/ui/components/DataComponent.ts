



export default function DataComponent<
    TData, 
    TTag extends keyof HTMLElementTagNameMap,
>(
    data: TData, tag: TTag
) {
    const result = document
        .createElement(tag) as DataElement<TData, HTMLElementTagNameMap[TTag]>;
    result.data = data;
    return result;
}

export type DataElement<T, TElement extends HTMLElement> = TElement & {
    data: T;
}