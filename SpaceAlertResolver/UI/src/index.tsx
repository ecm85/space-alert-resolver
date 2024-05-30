import { createRoot } from 'react-dom/client';
import { Router } from './Router';

function main() {
	const rootElement = document.getElementById('root');
	if (!rootElement) {
		throw new Error('Missing root DOM element.');
	}
	const rootComponent = <Router />;

	createRoot(rootElement).render(rootComponent);
}

main();
