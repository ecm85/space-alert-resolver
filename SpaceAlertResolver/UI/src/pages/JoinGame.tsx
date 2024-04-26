import React, { useEffect, useState } from 'react';
import { useWebSocket } from '~/hooks';
import { MessageEventData } from '~/models';

export function JoinGame() {
	const [gameCode, setGameCode] = useState<string>(null);
	const [name, setName] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();
	const [message, setMessage] = useState<string>(null);
	useEffect(() => {
		if (connectionStarted) {
			connection.onmessage = messageEvent => {
				const messageEventData = JSON.parse(messageEvent.data) as MessageEventData;
				switch (messageEventData.event) {
					case 'ExpiredGameCode':
						setMessage('That code is expired.');
						break;
					case 'InvalidGameCode':
						setMessage('That code is invalid.');
						break;
					case 'YouJoined':
						setMessage('You successfully joined the game!');
						break;
					default:
						console.log(`Event unhandled: ${messageEvent.data}`);
				}
			};
		}
	}, [connectionStarted]);

	const handleGameCodeChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
		setGameCode(event.target.value.toUpperCase());
	};

	const handleNameChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const canJoin = connectionStarted && gameCode?.length === 4 && !!name;

	const handleJoinClicked = () => {
		connection.send(JSON.stringify({ action: 'JoinGame', data: { name, code: gameCode } }));
	};

	return (
		<div>
			<h2>Join Game</h2>
			<div>
				<label>
					Your name:
					<input type='text' value={name} onChange={handleNameChanged} maxLength={50}></input>
				</label>
			</div>
			<div>
				<label>
					Game Code:
					<input
						type='text'
						value={gameCode}
						onChange={handleGameCodeChanged}
						maxLength={4}></input>
				</label>
			</div>
			<div>
				<button disabled={!canJoin} onClick={handleJoinClicked}>
					Join Game
				</button>
			</div>
			{message && <div>{message}</div>}
		</div>
	);
}
