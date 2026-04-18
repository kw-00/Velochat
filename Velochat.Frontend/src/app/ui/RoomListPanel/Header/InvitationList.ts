import invitationStore from "@/app/infrastructure/data-stores/invitation-store";
import type { Invitation } from "@/app/infrastructure/models";
import { StyleClass } from "@/dom-helpers/style-in-js";

const listStyle = new StyleClass("invitation-list", `
    overflow-y: scroll;
`);
listStyle.inject();
export default function InvitationList() {
    const invitationList = document.createElement("div");
    invitationList.className = `${listStyle.name} frame vs grow`;
    const listItems = invitationStore.get().map(invitation => InvitationListItem(invitation));

    return invitationList.appendAndGet(...listItems);
}

function InvitationListItem(invitation: Invitation) {
    const invitationLi = document.createElement("div");
    invitationLi.className = "item";
    invitationLi.textContent = `${invitation.roomName} (${invitation.roomOwnerLogin})`;
    return invitationLi;
}