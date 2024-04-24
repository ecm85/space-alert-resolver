import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { useEffect, useState } from 'react';

const apiByHost: Record<string, string> = {
	['localhost']: 'https://localhost:7001',
	['space-alert.stormtide.net']: 'https://space-alert-api.stormtide.net'
};

export const useHubConnection = () => {
	const [connection, setConnection] = useState<HubConnection>();
	const [connectionStarted, setConnectionStarted] = useState(false);

	function createHubConnection() {
		const host = window.location.hostname;

		const con = new HubConnectionBuilder()
			.withUrl(`${apiByHost[host]}/hub`, { withCredentials: false })
			.withAutomaticReconnect()
			.build();
		setConnection(con);
	}

	useEffect(() => {
		createHubConnection();
	}, []);

	const startConnection = async () => {
		await connection.start();
		setConnectionStarted(true);
	};

	useEffect(() => {
		if (connection) {
			startConnection();
		}

		return () => {
			connection?.stop();
		};
	}, [connection]);
	return {
		connection,
		connectionStarted
	};
};
