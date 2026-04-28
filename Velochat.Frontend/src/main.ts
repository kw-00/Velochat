


import 'normalize.css';
import 'reset.css';
import '@/style/style.css';
import "@/dom-helpers/element-extensions";
import Dashboard from "./app/ui/pages/dashboard/Dashboard";
import LoginPage from "./app/ui/pages/login/LoginPage";
import RegisterPage from "./app/ui/pages/register/RegisterPage";
import { InternalNavigation } from "./internal-navigation";
import { ServerInterface } from './app/infrastructure/server-interface';

const appElement = document.getElementById('app');
if (appElement === null) throw new Error("App element not found");

InternalNavigation.register("/", async () => {
	const refreshResponse = await ServerInterface.singleton
		.auth
		.refreshSessionAsync();

	if (refreshResponse.success) {
		InternalNavigation.goTo("/chat");
	} else {
		InternalNavigation.goTo("/login");
	}
});

InternalNavigation.register("/chat", async () => {
	appElement.replaceChildren(await Dashboard());
});

InternalNavigation.register("/login", () => {
	appElement.replaceChildren(LoginPage());
});

InternalNavigation.register("/register", () => {
	appElement.replaceChildren(RegisterPage());
});

InternalNavigation.resolve();