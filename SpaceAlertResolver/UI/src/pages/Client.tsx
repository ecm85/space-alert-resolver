import React, { useState } from 'react';
import { useWebSocket } from '~/hooks';
import { InputCards } from './InputCards';
import { JoinGame } from './JoinGame';

export function Client() {
	const [gameCode, setGameCode] = useState<string>(null);
	const { connection, connectionStarted } = useWebSocket();

	const handleGameJoined = (newGameCode: string) => {
		setGameCode(newGameCode);
	};

	return gameCode ? (
		<InputCards {...{ connection, gameCode }} />
	) : (
		<JoinGame {...{ connection, connectionStarted }} onGameJoined={handleGameJoined} />
	);
}
