


import 'normalize.css';
import 'reset.css';
import '@/style/style.css';
import "@/dom-helpers/element-extensions";
import Dashboard from "./app/ui/pages/dashboard/Dashboard";
import LoginPage from "./app/ui/pages/login/LoginPage";
import RegisterPage from "./app/ui/pages/register/RegisterPage";
import { InternalNavigation } from "./internal-navigation";

const appElement = document.getElementById('app');
if (appElement === null) throw new Error("App element not found");


InternalNavigation.register("/dashboard", () => {
	appElement.replaceChildren(Dashboard());
});

InternalNavigation.register("/login", () => {
	appElement.replaceChildren(LoginPage());
});

InternalNavigation.register("/register", () => {
	appElement.replaceChildren(RegisterPage());
});

InternalNavigation.resolve();