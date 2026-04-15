import { $ce } from "@/component-system/element-shortcuts";
import { StyleClass } from "@/component-system/style-in-js";


const style = new StyleClass("room-list-panel", `
    background-color: red;
`)
style.inject()

export default function RoomListPanel() {
    return $ce(
        "div", 
        style.name + " vs grow"
    )
}