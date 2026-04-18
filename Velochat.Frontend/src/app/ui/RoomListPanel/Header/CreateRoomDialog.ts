
import {  generateRandomId } from "@/dom-helpers/id-generation";



export default function CreateRoomDialog() {

    const dialog = document.createElement("dialog");
    dialog.className = "vs grow glow";
    dialog.style.position = "absolute";
    dialog.style.top = "10rem";


    const form = document.createElement("form");
    form.className = "frame vs grow gap-md px-lg";
    form.method = "dialog";
    form.addEventListener("submit", () => {

    });

    const nameInputLabel = document.createElement("label");
    nameInputLabel.htmlFor = generateRandomId().toString();;
    nameInputLabel.textContent = "Enter room name:";
    
    const nameInput = document.createElement("input");
    const nameInputId = generateRandomId().toString();
    nameInput.className = "input";
    nameInput.id = nameInputId;


    const createButton = document.createElement("button");
    createButton.className = "button-primary";
    createButton.type = "submit";
    createButton.textContent = "Create";

    const cancelButton = document.createElement("button");
    cancelButton.className = "button-danger";
    cancelButton.type = "reset";
    cancelButton.textContent = "Cancel";
    cancelButton.addEventListener("click", () => dialog.close());
        

    dialog.appendAndGet(
        form.appendAndGet(
            nameInputLabel,
            nameInput,
            createButton,
            cancelButton
        )
    );

    return dialog;
}