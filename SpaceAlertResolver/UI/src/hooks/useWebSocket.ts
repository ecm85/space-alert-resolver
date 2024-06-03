import { useEffect, useRef, useState } from 'react';

export const useWebSocket = () => {
	const connectionRef = useRef<WebSocket | null>(null);
	const [connectionStarted, setConnectionStarted] = useState(false);

	function createSocketConnection() {
		console.log('creating connection');
		const newConnection = new WebSocket(
			'wss://opc3otwjjk.execute-api.us-east-2.amazonaws.com/Live/',
		);
		newConnection.onopen = () => setConnectionStarted(true);
		connectionRef.current = newConnection;
	}

	useEffect(() => {
		createSocketConnection();
		return () => {
			connectionRef.current?.close();
		};
	}, [connectionRef]);

	return {
		connectionRef,
		connectionStarted,
	};
};
