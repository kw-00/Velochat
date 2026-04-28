import { InternalNavigation, internalPaths } from "@/internal-navigation";
import CredentialForm from "../../components/CredentialForm";
import InternalLink from "../../components/InternalLink";
import { ServerInterface } from "@/app/infrastructure/server-interface";


export default function RegisterPage() {
    const pageContainer = document.createElement("div");
    pageContainer.className = `page vs jc ac`;

    const form = CredentialForm(
        async (e, credentials) => {
            e.preventDefault();
            const { login, password } = credentials;
            const registrationResult = await ServerInterface.singleton
                .auth
                .registerAsync({ login, password });

            if (!registrationResult.success) {
                alert(registrationResult.message);
                return;
            }
            InternalNavigation.goTo(internalPaths.chat);         
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
