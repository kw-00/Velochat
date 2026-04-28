import type { Room } from "@/app/infrastructure/models";
import { ServerInterface } from "@/app/infrastructure/server-interface";
import DataComponent from "@/app/ui/components/DataComponent";
import DataListComponent from "@/app/ui/components/DataListComponent";
import { createElementWithCallbacks } from "@/dom-helpers/lifecycle";


const roomElementFactory = (room: Room) => {
    const roomElement = DataComponent(room, "div");
    roomElement.className = "item";
    roomElement.textContent = room.name;
    return roomElement;
};

export default function RoomsSection() {
    const outerContainer = createElementWithCallbacks();
    outerContainer.className = "panel vs grow";
    outerContainer.style.width = "20rem";

    const header = document.createElement("div");
    header.className = "panel hs jc";

    const createRoomPopup = CreateRoomPopup();

    const createRoomButton = document.createElement("button");
    createRoomButton.className = "button-secondary";
    createRoomButton.textContent = "Create room";
    createRoomButton.addEventListener("click", () => createRoomPopup.show());



    const roomsList = DataListComponent("div", roomElementFactory);
    roomsList.className = "frame vs grow scrollify csize";
    ServerInterface.singleton.rooms.on("added", rooms => roomsList.appendData(...rooms));
    ServerInterface.singleton.rooms.on(
        "removed", rooms => roomsList
            .removeData(r => rooms.map(rr => rr.id).includes(r.id))
    );
    ServerInterface.singleton.rooms.on(
        "overwritten", 
        (_, after) => {
            roomsList.reset(after);
        }
    );
    return (
        outerContainer.appendAndGet(
            header.appendAndGet(
                createRoomButton,
                createRoomPopup
            ),
            roomsList
        )
    );
}

function CreateRoomPopup() {
    const createRoomPopup = document.createElement("dialog");
    createRoomPopup.style.position = "absolute";

    const createRoomForm = document.createElement("form");
    createRoomForm.className = "panel vs gap-md px-lg";
    createRoomForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const data = new FormData(createRoomForm);
        const name = data.get("name")?.toString() ?? "";
        await ServerInterface.singleton.rooms.createRoomAsync(name);
    });

    const nameInput = document.createElement("input");
    nameInput.name = "name";
    nameInput.className = "input";

    const submitButton = document.createElement("button");
    submitButton.className = "button-primary";
    submitButton.textContent = "Create room";

    const cancelButton = document.createElement("button");
    cancelButton.type = "button";
    cancelButton.className = "button-danger";
    cancelButton.textContent = "Cancel";
    cancelButton.addEventListener("click", () => {
        createRoomForm.reset();
        createRoomPopup.close();
    });

    return (
        createRoomPopup.appendAndGet((
            createRoomForm.appendAndGet(
                nameInput,
                submitButton,
                cancelButton
            )
        ))
    ) as HTMLDialogElement;
}