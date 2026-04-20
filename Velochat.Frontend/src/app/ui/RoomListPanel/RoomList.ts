
import { OrchestratorInstance } from "@/app/infrastructure/orchestrator";
import type { Room } from "@/app/infrastructure/models";
import { createElementWithCallbacks } from "@/dom-helpers/lifecycle";



export default function RoomList() {
    const panel = createElementWithCallbacks();
    panel.className = "frame vs grow";

    const addOrUpdateScrollable = () => {
        const oldScrollable = panel.querySelector(`[data-scrollable]`);
        oldScrollable?.remove();

        const scrollable = document.createElement("div");
        scrollable.className = `vs grow scrollify csize`;
        scrollable.setAttribute("data-scrollable", "");
    
        const roomElements = 
            OrchestratorInstance.roomStore.get()
            .map(room => RoomListItem(room));

        panel.appendAndGet(
            scrollable.appendAndGet(
                ...roomElements
            )
        );
    };
    addOrUpdateScrollable();

    const unsubscribeFromStore = OrchestratorInstance.roomStore.subscribe(addOrUpdateScrollable);
    panel.setCallback("disconnectedCallback", unsubscribeFromStore);

    return panel;
}


function RoomListItem(room: Room) {
    const roomListItem = document.createElement("div");
    roomListItem.className = `item hs`;
    roomListItem.textContent = `${room.id} ${room.name}`;
    return roomListItem;
}

