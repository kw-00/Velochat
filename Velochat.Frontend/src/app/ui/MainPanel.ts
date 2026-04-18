import ChatPanel from "./ChatPanel/ChatPanel";
import RoomListPanel from "./RoomListPanel/RoomListPanel";
import { StyleClass } from "@/dom-helpers/style-in-js";

const style = new StyleClass("main-panel", `
    width: 100vw;
    height: 100vh;
`);
style.inject();

export default function MainPanel() {
    const mainPanel = document.createElement("div");
    mainPanel.className = `${style.name} app-pane hs`;
    
    mainPanel.appendAndGet(
        RoomListPanel(),
        ChatPanel()
    );
    return mainPanel;
}