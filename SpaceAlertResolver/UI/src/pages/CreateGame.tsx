import { Skeleton } from '@mui/material';
import Typography from '@mui/material/Typography';
import React, { useEffect, useState } from 'react';
import { useStateRef, useWebSocket } from '~/hooks';
import { useConnectionSubscription } from '~/hooks/useConnectionSubscription';
import { Client, MessageEventData } from '~/models';
import styles from './CreateGame.css';

export function CreateGame() {
	const [gameCode, setGameCode] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();
	const [clients, setClients, clientsRef] = useStateRef<Client[]>([]);
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
				setClients([...clientsRef.current, { ...messageEventData.data }]);
				return true;
			case 'ClientMessageReceived': {
				const connectionId = messageEventData.data.connectionId;
				const updatedClient = clientsRef.current.filter(
					client => client.connectionId === connectionId
				)[0];
				const otherClients = clientsRef.current.filter(
					client => client.connectionId !== connectionId
				);
				setClients([
					...otherClients,
					{
						...updatedClient,
						...messageEventData.data
					}
				]);
				return true;
			}
			default:
				return false;
		}
	};

	const { isSubscribed } = useConnectionSubscription({
		onMessage: handleMessage,
		connection,
		connectionStarted
	});

	useEffect(() => {
		if (isSubscribed) {
			startGame();
		}
	}, [isSubscribed]);

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
					<ul>
						{clients.map(client => (
							<li>
								<>
									{client.name}
									<ul>
										{client.barcodeData?.map((scannedCard, index) => (
											<li>
												Barcode {index + 1}: {scannedCard}
											</li>
										))}
									</ul>
								</>
							</li>
						))}
					</ul>
				</div>
			)}
		</div>
	);
}
