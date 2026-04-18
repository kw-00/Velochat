import MainPanel from './app/ui/MainPanel';
import 'normalize.css';
import 'reset.css';
import '@/style/style.css';
import "@/dom-helpers/element-extensions";

document.getElementById('app')?.append(
	MainPanel()
);

