import type { Invitation } from "@/app/infrastructure/models";
import { OrchestratorInstance } from "@/app/infrastructure/orchestrator";
import { createElementWithCallbacks } from "@/dom-helpers/lifecycle";


export default function InvitationList() {
    const outerContainer = createElementWithCallbacks();
    outerContainer.className = `frame vs grow`;

    let nearTop = true;
    let scrollTop = 0;
    let scrollFromBottom = 0;
    let userIsScrolling = false;

    const addOrUpdateScrollable = () => {
        const oldScrollable = outerContainer.querySelector(`[data-scrollable]`);
        oldScrollable?.remove();

        const scrollable = document.createElement("div");
        scrollable.className = `vs grow scrollify csize`;
        scrollable.setAttribute("data-scrollable", "");

        const invitationElements = OrchestratorInstance
            .invitationStore
            .get()
            .map(invitation => InvitationListItem(invitation));
        
        outerContainer.appendAndGet(
            scrollable.appendAndGet(
                ...invitationElements.reverse()
            )
        );

        if (!nearTop) {
            if (!userIsScrolling) {
                scrollable.scrollTo({
                    top: scrollable.scrollHeight - scrollable.clientHeight - scrollFromBottom,
                    behavior: "instant"
                });
            } else {
                scrollable.scrollTo({
                    top: scrollTop,
                    behavior: "smooth"
                });
            }
        }
        scrollable.addEventListener("scroll", () => {
            scrollTop = scrollable.scrollTop;
            scrollFromBottom 
                = scrollable.scrollHeight - scrollable.clientHeight - scrollable.scrollTop;
            nearTop = scrollable.scrollTop === 0;
        });

        scrollable.addEventListener("pointerdown", () => userIsScrolling = true);
        scrollable.addEventListener("pointerup", () => userIsScrolling = false);
    };
    addOrUpdateScrollable();

    const unsubscribeFromStore = OrchestratorInstance
        .invitationStore
        .subscribe(addOrUpdateScrollable);
    outerContainer.setCallback("disconnectedCallback", unsubscribeFromStore);;

    return outerContainer;
}

function InvitationListItem(invitation: Invitation) {
    const invitationLi = document.createElement("div");
    invitationLi.className = "item";
    invitationLi.textContent = `${invitation.roomName} (${invitation.roomOwnerLogin})`;
    return invitationLi;
}