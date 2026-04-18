
import { StyleClass } from "@/dom-helpers/style-in-js";
import CreateRoomDialog from "./CreateRoomDialog";
import InvitationList from "./InvitationList";

const headerStyle = new StyleClass("header-bar", `
    justify-content: flex-end;
`);
headerStyle.inject();

export default function HeaderBar() {
    const headerBar = document.createElement("div");
    headerBar.className = `${headerStyle.name} panel hs gap-sm`;

    const invitationListDialog = InvtiationListDialog();

    const invitationsButton = document.createElement("button");
    invitationsButton.className = "button-secondary";
    invitationsButton.textContent = "Invitations";
    invitationsButton.addEventListener("click", () => invitationListDialog.showModal());
    
    const createRoomDialog = CreateRoomDialog();

    const createRoomButton = document.createElement("button");
    createRoomButton.className = "button-secondary";
    createRoomButton.textContent = "Create Room";
    createRoomButton.addEventListener("click", () => createRoomDialog.show());

    headerBar.appendAndGet(
        invitationListDialog,
        invitationsButton,

        createRoomDialog,
        createRoomButton
    );
    return headerBar;
}

function InvtiationListDialog() {
    const invitationListDialog = document.createElement("dialog");
    invitationListDialog.className = "full-overlay";
    invitationListDialog.style.flexDirection = "column";
    
    const panel = document.createElement("div");
    panel.className = "panel vs grow gap-md";
    panel.style.maxHeight = "70vh";
    
    const closeButton = document.createElement("button");
    closeButton.className = "button-secondary";
    closeButton.textContent = "Close";
    closeButton.addEventListener("click", () => invitationListDialog.close());

    invitationListDialog.appendAndGet(
        panel.appendAndGet(
            InvitationList(),
            closeButton
        )
    );
    return invitationListDialog;
}