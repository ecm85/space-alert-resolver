import { Button } from '@mui/material';
import TextField from '@mui/material/TextField';
import React, { useEffect, useState } from 'react';
import { useStateRef } from '~/hooks';
import { MessageEventData } from '~/models';
import styles from './JoinGame.css';

export interface JoinGameProps {
	connectionStarted: boolean;
	connection: WebSocket;
	onGameJoined(gameCode: string): void;
}

export function JoinGame({ connectionStarted, connection, onGameJoined }: JoinGameProps) {
	const [gameCode, setGameCode, gameCodeRef] = useStateRef<string>(null);
	const [name, setName] = useState<string>(null);
	const [message, setMessage] = useState<string>(null);
	const [joining, setJoining] = useState(false);
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
						onGameJoined(gameCodeRef.current);
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

	const canJoin = connectionStarted && gameCode?.length === 4 && !!name && !joining;

	const handleJoinClicked = () => {
		setJoining(true);
		connection.send(JSON.stringify({ action: 'JoinGame', data: { name, code: gameCode } }));
	};

	return (
		<div>
			<h2>Join Game</h2>
			<div className={styles['inputs']}>
				<TextField label='Your name:' value={name} onChange={handleNameChanged}></TextField>
				<TextField label='Game Code:' value={gameCode} onChange={handleGameCodeChanged}></TextField>
				<Button variant='contained' disabled={!canJoin} onClick={handleJoinClicked}>
					{!joining ? 'Join Game' : 'Joining...'}
				</Button>
			</div>
			{message && <div>{message}</div>}
		</div>
	);
}
