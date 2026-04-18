import { StyleClass } from "@/dom-helpers/style-in-js";


const style = new StyleClass("chat-panel", `
`);
style.inject();

export default function ChatPanel() {

    const chatPanel = document.createElement("div");
    chatPanel.className = `${style.name} surface vs grow`;
    return chatPanel;
}