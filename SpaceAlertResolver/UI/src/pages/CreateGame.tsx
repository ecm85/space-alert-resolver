import { Skeleton } from '@mui/material';
import Typography from '@mui/material/Typography';
import React, { useEffect, useState } from 'react';
import { useStateRef, useWebSocket } from '~/hooks';
import { Client, MessageEventData } from '~/models';
import styles from './CreateGame.css';

export function CreateGame() {
	const [gameCode, setGameCode] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();
	const [clients, setClients, clientsRef] = useStateRef<Client[]>([]);
	const [barcodeDataByConnectionId, setBarcodeDataByConnectionId, barcodeDataByConnectionIdRef] =
		useStateRef<Record<string, string[][]>>({});
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
			case 'ClientMessageReceived':
				setBarcodeDataByConnectionId({
					...barcodeDataByConnectionIdRef.current,
					[messageEventData.data.connectionId]: messageEventData.data.barcodeData
				});
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
					<ul>
						{clients.map(client => (
							<li>
								<>
									{client.name}
									<ul>
										{barcodeDataByConnectionId[client.connectionId]?.map((barcodeData, index) => (
											<li>
												Barcode {index + 1}:{' '}
												{barcodeData.map((barcodePiece, index) => (
													<span>
														{index > 0 && <span>-</span>}
														{barcodePiece}
													</span>
												))}
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
