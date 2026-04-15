import { $ce } from "@/component-system/element-shortcuts";
import ChatPanel from "./ChatPanel/ChatPanel";
import RoomListPanel from "./RoomListPanel/RoomListPanel";
import { StyleClass } from "@/component-system/style-in-js";

const style = new StyleClass("main-panel", `
    width: 100vw;
    height: 100vh;
`)
style.inject()

export default function MainPanel() {
    const mainPanel = $ce(
        "div",
        style.name + " hs",
        RoomListPanel(),
        ChatPanel()
    )
    return mainPanel;
}