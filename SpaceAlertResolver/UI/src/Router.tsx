import React from 'react';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import { Client, CreateGame, Home } from '~/pages';
import { PageFrame } from './components/PageFrame';

const router = createBrowserRouter([
	{
		path: '/',
		// errorElement: <ErrorView />,
		element: <PageFrame />,
		children: [
			{ path: '/', element: <Home /> },
			{ path: '/client', element: <Client /> },
			{ path: '/create', element: <CreateGame /> }
		]
	}
]);

export function Router() {
	return <RouterProvider router={router} />;
}
