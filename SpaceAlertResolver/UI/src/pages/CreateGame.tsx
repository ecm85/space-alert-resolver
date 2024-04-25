import React, { useEffect, useState } from 'react';
import { useWebSocket } from '~/hooks';

interface CreatedGameEvent {
	event: 'GameCreated';
	data: {
		code: string;
	};
}

interface InvalidGameCodeEvent {
	event: 'InvalidGameCode';
}

interface ExpiredGameCodeEvent {
	event: 'ExpiredGameCode';
}

interface YouJoinedEvent {
	event: 'YouJoined';
}

interface ClientJoinedEvent {
	event: 'ClientJoined';
	data: {
		name: string;
	};
}

interface ClientMessageReceivedEvent {
	event: 'ClientMessageReceived';
	data: {
		text: string;
	};
}

interface YourMessageSentEvent {
	event: 'YourMessageSent';
}

type MessageEventData =
	| CreatedGameEvent
	| InvalidGameCodeEvent
	| ExpiredGameCodeEvent
	| YouJoinedEvent
	| ClientJoinedEvent
	| ClientMessageReceivedEvent
	| YourMessageSentEvent;

export function CreateGame() {
	const [gameCode, setGameCode] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();
	const startGame = async () => {
		connection.send(JSON.stringify({ body: { action: 'CreateGame' } }));
	};
	useEffect(() => {
		if (connectionStarted) {
			connection.onmessage = messageEvent => {
				const messageEventData = JSON.parse(messageEvent.data) as MessageEventData;
				switch (messageEventData.event) {
					case 'GameCreated':
						setGameCode(messageEventData.data.code);
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
			<div>Connection Started: {connectionStarted.toString()}</div>
			{gameCode && <div>Game Code: {gameCode}</div>}
		</div>
	);
}
