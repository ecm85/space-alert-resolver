import React from 'react';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import { CreateGame, Home, JoinGame } from '~/pages';

const router = createBrowserRouter([
	{
		path: '/',
		// errorElement: <ErrorView />,
		children: [
			{ path: '/', element: <Home /> },
			{ path: '/join', element: <JoinGame /> },
			{ path: '/create', element: <CreateGame /> }
			// { path: '/auth', element: <AuthOwners /> },
		]
	}
]);

export function Router() {
	return <RouterProvider router={router} />;
}
