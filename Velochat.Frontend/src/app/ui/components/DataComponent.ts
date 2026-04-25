


export type DataComponentType<T, TElement extends HTMLElement> = TElement & {
    data: T;
}

export default function DataComponent<
    TData, 
    TTag extends keyof HTMLElementTagNameMap,
>(
    data: TData, tag: TTag
) {
    const result = document
        .createElement(tag) as DataComponentType<TData, HTMLElementTagNameMap[TTag]>;
    result.data = data;
    return result;
}
