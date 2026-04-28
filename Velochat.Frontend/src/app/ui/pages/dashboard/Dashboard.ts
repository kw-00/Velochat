import { ServerInterface } from "@/app/infrastructure/server-interface";
import RoomsSection from "./RoomsSection/RoomsSection";




export default async function Dashboard() {
    const mainPanel = document.createElement("div");
    mainPanel.className = `page hs`;
    
    ServerInterface.singleton.startRealtimeSessionAsync();
    return (
        mainPanel.appendAndGet(
            RoomsSection()
        )
    );
}