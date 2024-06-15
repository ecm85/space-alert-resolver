import { Skeleton } from '@mui/material';
import Typography from '@mui/material/Typography';
import { useCallback, useEffect, useState } from 'react';
import { useStateRef, useWebSocket } from '~/hooks';
import { useConnectionSubscription } from '~/hooks/useConnectionSubscription';
import { Client, MessageEventData } from '~/models';
import styles from './CreateGame.module.css';
import { useSendToHostMessaging } from '~/hooks/useSendToHostMessaging';
import { PlayerBoard } from '~/components/PlayerBoard';

export function CreateGame() {
	const [gameCode, setGameCode] = useState<string | null>(null);
	const { connectionRef, connectionStarted } = useWebSocket();
	const [clients, setClients, clientsRef] = useStateRef<Client[]>([]);
	const isLoading = !gameCode;
	const { deserialize } = useSendToHostMessaging();

	const handleMessage = useCallback(
		(messageEventData: MessageEventData) => {
			switch (messageEventData.event) {
				case 'GameCreated':
					setGameCode(messageEventData.data.code);
					return true;
				case 'ClientJoined':
					setClients([...clientsRef.current, { ...messageEventData.data }]);
					return true;
				case 'ClientMessageReceived': {
					const connectionId = messageEventData.data.connectionId;
					const clientToUpdate = clientsRef.current.filter(
						(client) => client.connectionId === connectionId,
					)[0];
					const otherClients = clientsRef.current.filter(
						(client) => client.connectionId !== connectionId,
					);
					const { message, messageType } = messageEventData.data;
					const { scannedCards, playerColor } = deserialize(message, messageType);
					const updatedClient: Client = {
						...clientToUpdate,
						scannedCards,
						playerColor,
					};
					setClients([...otherClients, updatedClient]);
					return true;
				}
				default:
					return false;
			}
		},
		[clientsRef, setGameCode, setClients, deserialize],
	);

	const { isSubscribed } = useConnectionSubscription({
		onMessage: handleMessage,
		connectionRef,
		connectionStarted,
	});

	useEffect(() => {
		const startGame = async () => {
			connectionRef.current?.send(JSON.stringify({ action: 'CreateGame' }));
		};
		if (isSubscribed) {
			startGame();
		}
	}, [isSubscribed, connectionRef]);

	return (
		<div>
			<h2>Create Game</h2>
			<div>
				<Typography variant="body1">Game Code: </Typography>
				{isLoading ? (
					<Skeleton variant="text" className={styles['skeleton']} />
				) : (
					<div>
						<Typography variant="h5">{gameCode}</Typography>
					</div>
				)}
			</div>
			{clients.length > 0 && (
				<div>
					<h3>Clients</h3>
					<ul>
						{clients.map((client) => (
							<li key={client.connectionId}>
								<>
									{client.name}
									{client.scannedCards != null && client.playerColor != null && (
										<PlayerBoard
											scannedCards={client.scannedCards}
											playerColor={client.playerColor}
										/>
									)}
								</>
							</li>
						))}
					</ul>
				</div>
			)}
		</div>
	);
}
