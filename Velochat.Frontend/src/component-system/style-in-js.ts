

const globalStyle = document.createElement("style")
globalStyle.id = "global-style"
document.head.append(globalStyle)

export class StyleClass {
    _alreadyInjected = false
    _hashedName: string
    _styles: string[] = []

    constructor(name: string, css?: string) {
        const randomNumber = crypto.getRandomValues(new Uint16Array(1))[0];
        this._hashedName = `b${randomNumber}_${name}`
        if (css != undefined) this.add("", css)
    }

    add(selector: string, css: string) {
        this._styles.push(`
            .${this._hashedName}${selector} {
                ${css}
            }
        `)
    }

    inject() {
        const css = this._styles.join("\n\n")
        globalStyle.textContent += "\n\n" + css
    }

    get name() {
        return this._hashedName
    }
}

