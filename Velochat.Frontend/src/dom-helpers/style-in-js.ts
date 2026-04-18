import { getRandomClassPrefix } from "./id-generation";


const globalStyle = document.createElement("style");
globalStyle.id = "global-style";
document.head.append(globalStyle);

export class StyleClass {
    private _alreadyInjected = false;
    private _nameWithPrefix: string;
    private _styles: string[] = [];

    constructor(name: string, css?: string) {
        const randomPrefix = getRandomClassPrefix();
        this._nameWithPrefix = `${randomPrefix}_${name}`;
        if (css != undefined) this.add("", css);
    }

    add(selector: string, css: string) {
        this._styles.push(`
            .${this._nameWithPrefix}${selector} {
                ${css}
            }
        `);
    }

    inject() {
        if (this._alreadyInjected) return;
        const css = this._styles.join("\n\n");
        globalStyle.textContent += "\n\n" + css;
        this._alreadyInjected = true;
    }

    get name() {
        return this._nameWithPrefix;
    }
}

