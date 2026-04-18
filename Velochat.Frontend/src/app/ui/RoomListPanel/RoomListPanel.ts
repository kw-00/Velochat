
import { StyleClass } from "@/dom-helpers/style-in-js";
import RoomList from "./RoomList";
import HeaderBar from "./Header/HeaderBar";


const style = new StyleClass("room-list-panel", `
`);
style.inject();

export default function RoomListPanel() {
    const roomListPanel = document.createElement("div");
    roomListPanel.className = `${style.name} surface vs grow`;

    roomListPanel.appendAndGet(
        HeaderBar(),
        RoomList()
    );
    return roomListPanel;
}

