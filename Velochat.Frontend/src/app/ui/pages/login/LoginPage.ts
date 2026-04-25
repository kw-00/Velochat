import { internalPaths } from "@/internal-navigation";
import CredentialForm from "../../components/CredentialForm";
import InternalLink from "../../components/InternalLink";




export default function LoginPage() {
    const pageContainer = document.createElement("div");
    pageContainer.className = `page vs jc ac`;

    const form = CredentialForm(
        (e, credentials) => {
            e.preventDefault();
            console.log(credentials);
            // TODO - login event listener
        },
        "Log in"
    );

    const registerInsteadElement = document.createElement("p");
    registerInsteadElement.innerHTML = "Don't have an account? ";
    const registerLink = InternalLink(internalPaths.register, "Register");
    registerInsteadElement.style.textAlign = "center";

    return (
        pageContainer.appendAndGet(
            form.appendAndGet(
                registerInsteadElement.appendAndGet(
                    registerLink
                )
            )
        )
    );
}