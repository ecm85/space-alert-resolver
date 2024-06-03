import { useState } from 'react';
import { useWebSocket } from '~/hooks';
import { InputCards } from './InputCards';
import { JoinGame } from './JoinGame';

export function Client() {
	const [gameCode, setGameCode] = useState<string | null>(null);
	const { connectionRef, connectionStarted } = useWebSocket();

	const handleGameJoined = (newGameCode: string) => {
		setGameCode(newGameCode);
	};

	return gameCode ? (
		<InputCards {...{ connectionRef, gameCode }} />
	) : (
		<JoinGame {...{ connectionRef, connectionStarted }} onGameJoined={handleGameJoined} />
	);
}
