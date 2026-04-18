
import globalRoomStore from "@/app/infrastructure/data-stores/room-store";
import type { Room } from "@/app/infrastructure/models";
import { StyleClass } from "@/dom-helpers/style-in-js";


const roomContainerStyle = new StyleClass("room-list", `
    overflow-y: scroll;
    contain: size;
`);
roomContainerStyle.inject();

export default function RoomList() {
    const panel = document.createElement("div");
    panel.className = "frame vs grow";

    const scrollable = document.createElement("div");
    scrollable.className = `${roomContainerStyle.name} vs grow`;

    const roomElements = 
        [...globalRoomStore.get()]
        .map(room => RoomListItem(room));

    panel.appendAndGet(
        scrollable.appendAndGet(
            ...roomElements
        )
    );
    return panel;
}

const itemStyle = new StyleClass("room-list-item", `

`);
itemStyle.inject();

function RoomListItem(room: Room) {
    const roomListItem = document.createElement("div");
    roomListItem.className = `${itemStyle.name} item hs`;
    roomListItem.textContent = `${room.id} ${room.name}`;
    return roomListItem;
}