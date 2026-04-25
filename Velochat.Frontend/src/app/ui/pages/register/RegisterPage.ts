import { internalPaths } from "@/internal-navigation";
import CredentialForm from "../../components/CredentialForm";
import InternalLink from "../../components/InternalLink";


export default function RegisterPage() {
    const pageContainer = document.createElement("div");
    pageContainer.className = `page vs jc ac`;

    const form = CredentialForm(
        (e, credentials) => {
            e.preventDefault();
            console.log(credentials);
            // TODO - register event listener
        },
        "Register"
    );

    const loginInsteadElement = document.createElement("p");
    loginInsteadElement.innerHTML = "Already have an account? ";
    const loginLink = InternalLink(internalPaths.login, "Log in");
    loginInsteadElement.style.textAlign = "center";


    return (
        pageContainer.appendAndGet(
            form.appendAndGet(
                loginInsteadElement.appendAndGet(
                    loginLink
                )
            )
        )
    );
}
