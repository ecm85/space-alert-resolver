import React from 'react';
import { createRoot } from 'react-dom/client';
import Application from '~/Application';

function main() {
	const rootElement = document.getElementById('root');
	if (!rootElement) {
		throw new Error('Missing root DOM element.');
	}

	const rootComponent = <Application />;

	createRoot(rootElement).render(rootComponent);
}

main();
