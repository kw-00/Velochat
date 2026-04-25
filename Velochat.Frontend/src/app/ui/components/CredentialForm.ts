import type { Credentials } from "@/app/infrastructure/models";




export default function CredentialForm(
    onSubmit: (e: SubmitEvent, credentials: Credentials) => void,
    submitButtonText: string
) {
    const form = document.createElement("form");
    form.className = "panel vs gap-md px-lg";
    form.style.width = "20rem";
    form.addEventListener("submit", e => {
        const data = new FormData(form);
        const login = data.get("login")?.toString();
        const password = data.get("password")?.toString();
        if (login === undefined || password === undefined)
            throw new Error(
                "Login and password inputs are expected, yet absent."
                + " Did you remove them from the DOM?"
            );

        onSubmit(e, { login, password });

    });

    const loginInput = document.createElement("input");
    loginInput.className = "input";
    loginInput.name = "login";
    loginInput.type = "text";
    loginInput.placeholder = "Enter login...";

    const passwordInput = document.createElement("input");
    passwordInput.className = "input";
    passwordInput.name = "password";
    passwordInput.type = "password";
    passwordInput.placeholder = "Enter password...";

    const loginButton = document.createElement("button");
    loginButton.className = "button-primary";
    loginButton.type = "submit";
    loginButton.textContent = submitButtonText;

    return (
        form.appendAndGet(
            loginInput,
            passwordInput,
            loginButton
        )
    );
}