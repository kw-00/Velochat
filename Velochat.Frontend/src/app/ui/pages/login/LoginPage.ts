import { InternalNavigation, internalPaths } from "@/internal-navigation";
import CredentialForm from "../../components/CredentialForm";
import InternalLink from "../../components/InternalLink";
import { ServerInterface } from "@/app/infrastructure/server-interface";




export default function LoginPage() {
    const pageContainer = document.createElement("div");
    pageContainer.className = `page vs jc ac`;

    const form = CredentialForm(
        async (e, credentials) => {
            e.preventDefault();
            const {login, password} = credentials;
            const authenticationResult = await ServerInterface.singleton
                .identity
                .logInAsync({login, password});
                
            if (authenticationResult.success) {
                InternalNavigation.goTo(internalPaths.dashboard);
            } else {
                alert(authenticationResult.message);
            }
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