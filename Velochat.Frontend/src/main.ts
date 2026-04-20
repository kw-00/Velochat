import MainPanel from './app/ui/MainPanel';
import 'normalize.css';
import 'reset.css';
import '@/style/style.css';
import "@/dom-helpers/element-extensions";
import { OrchestratorInstance } from './app/infrastructure/orchestrator';

document.getElementById('app')?.append(
	MainPanel()
);



const add = () => {
	OrchestratorInstance.invitationStore.add({
		roomId: 1,
		roomName: "string",
		roomOwnerId: 1,
		roomOwnerLogin: "string"
	});
	setTimeout(add, 1000);
};

add();

