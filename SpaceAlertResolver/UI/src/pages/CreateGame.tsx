import React, { useEffect, useState } from 'react';
import { useWebSocket } from '~/hooks';
import { MessageEventData } from '~/models';

export function CreateGame() {
	const [gameCode, setGameCode] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();
	const [clients, setClients] = useState<string[]>([]);
	const [newClient, setNewClient] = useState<string>(null);
	const startGame = async () => {
		connection.send(JSON.stringify({ action: 'CreateGame' }));
	};

	const handleMessage = (messageEventData: MessageEventData) => {
		switch (messageEventData.event) {
			case 'GameCreated':
				setGameCode(messageEventData.data.code);
				return true;
			case 'ClientJoined':
				setNewClient(messageEventData.data.name);
				return true;
			default:
				return false;
		}
	};

	useEffect(() => {
		if (newClient) {
			setClients([...clients, newClient]);
			setNewClient(null);
		}
	}, [newClient]);

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	const onMessage = (messageEvent: MessageEvent<any>) => {
		const messageEventData = JSON.parse(messageEvent.data) as MessageEventData;
		const success = handleMessage(messageEventData);
		if (!success) {
			console.log(`Event unhandled: ${messageEvent.data}`);
		}
	};
	useEffect(() => {
		if (connectionStarted) {
			connection.addEventListener('message', onMessage);
			// connection.onmessage = onMessage;
			startGame();
		}
	}, [connectionStarted]);
	return (
		<div>
			<h2>Create Game</h2>
			{gameCode && <div>Game Code: {gameCode}</div>}
			{clients.length > 0 && (
				<div>
					<h3>Clients</h3>
					{clients.map(client => (
						<div>{client}</div>
					))}
				</div>
			)}
		</div>
	);
}
