import React, { useEffect, useState } from 'react';
import { useWebSocket } from '~/hooks';
import { MessageEventData } from '~/models';

export function CreateGame() {
	const [gameCode, setGameCode] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();
	const [clients, setClients] = useState<string[]>([]);
	const startGame = async () => {
		connection.send(JSON.stringify({ action: 'CreateGame' }));
	};
	useEffect(() => {
		if (connectionStarted) {
			connection.onmessage = messageEvent => {
				const messageEventData = JSON.parse(messageEvent.data) as MessageEventData;
				switch (messageEventData.event) {
					case 'GameCreated':
						setGameCode(messageEventData.data.code);
						break;
					case 'ClientJoined':
						setClients([...clients, messageEvent.data.name]);
						break;
					default:
						console.log(`Event unhandled: ${messageEvent.data}`);
				}
			};

			startGame();
		}
	}, [connectionStarted]);
	return (
		<div>
			<h2>Create Game</h2>
			{gameCode && <div>Game Code: {gameCode}</div>}
			{clients.length && (
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
