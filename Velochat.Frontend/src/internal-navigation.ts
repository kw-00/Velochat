import Navigo from "navigo";


const navigo = new Navigo("/");


export const internalPaths = {
    landing: "/",
    dashboard: "/dashboard",
    login: "/login",
    register: "/register"
} as const;

export type InternalPath = typeof internalPaths[keyof typeof internalPaths];

export class InternalNavigation {
    static register(route: InternalPath, hook: () => void) {
        navigo.on(route, hook);
    }

    static goTo(path: InternalPath) {
        navigo.navigate(path);
    }

    static resolve() {
        navigo.resolve();
    }
}