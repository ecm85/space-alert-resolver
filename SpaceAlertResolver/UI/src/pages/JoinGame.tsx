import { Button } from '@mui/material';
import TextField from '@mui/material/TextField';
import React, { useState } from 'react';
import { useStateRef } from '~/hooks';
import { useConnectionSubscription } from '~/hooks/useConnectionSubscription';
import { MessageEventData } from '~/models';
import styles from './JoinGame.module.css';

export interface JoinGameProps {
	connectionStarted: boolean;
	connection: WebSocket | null;
	onGameJoined(gameCode: string): void;
}

export function JoinGame({ connectionStarted, connection, onGameJoined }: JoinGameProps) {
	const [gameCode, setGameCode, gameCodeRef] = useStateRef<string | null>(null);
	const [name, setName] = useState<string | null>(null);
	const [message, setMessage] = useState<string | null>(null);
	const [joining, setJoining] = useState(false);
	const handleMessage = (messageEventData: MessageEventData) => {
		switch (messageEventData.event) {
			case 'ExpiredGameCode':
				setMessage('That code is expired.');
				return true;
			case 'InvalidGameCode':
				setMessage('That code is invalid.');
				return true;
			case 'YouJoined':
				if (gameCodeRef.current != null) {
					onGameJoined(gameCodeRef.current);
				}
				return true;
			default:
				return false;
		}
	};

	const { isSubscribed } = useConnectionSubscription({
		onMessage: handleMessage,
		connection,
		connectionStarted,
	});

	const handleGameCodeChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
		setGameCode(event.target.value.toUpperCase());
	};

	const handleNameChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const canJoin = connectionStarted && isSubscribed && gameCode?.length === 4 && !!name && !joining;

	const handleJoinClicked = () => {
		setJoining(true);
		connection?.send(JSON.stringify({ action: 'JoinGame', data: { name, code: gameCode } }));
	};

	return (
		<div>
			<h2>Join Game</h2>
			<div className={styles['inputs']}>
				<TextField label="Your name:" value={name} onChange={handleNameChanged}></TextField>
				<TextField label="Game Code:" value={gameCode} onChange={handleGameCodeChanged}></TextField>
				<Button variant="contained" disabled={!canJoin} onClick={handleJoinClicked}>
					{!joining ? 'Join Game' : 'Joining...'}
				</Button>
			</div>
			{message && <div>{message}</div>}
		</div>
	);
}
