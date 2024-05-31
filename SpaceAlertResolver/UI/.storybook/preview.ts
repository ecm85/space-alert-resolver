import { createElement } from 'react';
import type { Preview } from '@storybook/react';
import { MemoryRouter } from 'react-router-dom';
import { initialize, mswLoader } from 'msw-storybook-addon';
import { action as storybookAction } from '@storybook/addon-actions';

initialize({
	onUnhandledRequest: 'bypass',
	serviceWorker: {
		url: './mockServiceWorker.js',
	},
})
	.events.on('request:unhandled', ({ request }) => {
		if (request.url.includes('localhost6511')) {
			storybookAction('network')({ url: request.url });
		}
	})
	.on('response:mocked', async ({ request, response }) => {
		storybookAction('network')({
			url: request.url,
			requestBody: request.body !== null ? await request.json() : null,
			responseStatus: response.status,
			responseBody:
				response.body !== null
					? response.headers.get('Content-Type') === 'application/json'
						? await response.json()
						: await response.text()
					: null,
		});
	});

const preview: Preview = {
	parameters: {
		actions: { argTypesRegex: '^on[A-Z].*' },
	},
	decorators: [
		(Story, context) => {
			return createElement(MemoryRouter, {}, createElement(Story, context as any));
		},
	],
	loaders: [mswLoader],
};

export default preview;
