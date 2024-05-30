import { useEffect, useState } from 'react';

export const useWebSocket = () => {
	const [connection, setConnection] = useState<WebSocket | null>(null);
	const [connectionStarted, setConnectionStarted] = useState(false);

	function createSocketConnection() {
		const newConnection = new WebSocket(
			'wss://opc3otwjjk.execute-api.us-east-2.amazonaws.com/Live/',
		);
		newConnection.onopen = () => setConnectionStarted(true);
		setConnection(newConnection);
	}

	useEffect(() => {
		createSocketConnection();
		return () => {
			connection?.close();
		};
	}, []);

	return {
		connection,
		connectionStarted,
	};
};
