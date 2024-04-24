import React, { useEffect, useState } from 'react';
import { useHubConnection } from '~/hooks';

export function CreateGame() {
	const [gameCode, setGameCode] = useState<number>(null);
	const { connectionStarted, connection } = useHubConnection();
	const startGame = async () => {
		connection.invoke('StartGame');
	};
	useEffect(() => {
		if (connectionStarted) {
			connection.on('GameCreated', setGameCode);
			startGame();
		}
	}, [connectionStarted]);
	return (
		<div>
			<h2>Create Game</h2>Connection Started: {connectionStarted.toString()}
			{gameCode && <div>Game Code: {gameCode}</div>}
		</div>
	);
}
