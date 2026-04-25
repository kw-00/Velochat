import { InternalNavigation, type InternalPath } from "@/internal-navigation";




export default function InternalLink(href: InternalPath, text: string) {
    const link = document.createElement("a");
    link.href = href;
    link.textContent = text;
    link.addEventListener("click", e => {
        e.preventDefault();
        InternalNavigation.goTo(href);
    });
    return link;
}