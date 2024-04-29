import { Skeleton } from '@mui/material';
import Typography from '@mui/material/Typography';
import React, { useEffect, useState } from 'react';
import { useStateRef, useWebSocket } from '~/hooks';
import { MessageEventData } from '~/models';
import styles from './CreateGame.css';

export function CreateGame() {
	const [gameCode, setGameCode] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();
	const [clients, setClients, clientsRef] = useStateRef<string[]>([]);
	const startGame = async () => {
		connection.send(JSON.stringify({ action: 'CreateGame' }));
	};
	const isLoading = !gameCode;

	const handleMessage = (messageEventData: MessageEventData) => {
		switch (messageEventData.event) {
			case 'GameCreated':
				setGameCode(messageEventData.data.code);
				return true;
			case 'ClientJoined':
				setClients([...clientsRef.current, messageEventData.data.name]);
				return true;
			default:
				return false;
		}
	};

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
			startGame();
		}
	}, [connectionStarted]);
	return (
		<div>
			<h2>Create Game</h2>
			<div>
				<Typography variant='body1'>
					Game Code:{' '}
					{isLoading ? (
						<Skeleton variant='text' className={styles['skeleton']} />
					) : (
						<div>
							<Typography variant='h5'>{gameCode}</Typography>
						</div>
					)}
				</Typography>
			</div>
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
