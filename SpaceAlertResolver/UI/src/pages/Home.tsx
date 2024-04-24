import React from 'react';
import { useNavigate } from 'react-router-dom';

export function Home() {
	const navigate = useNavigate();

	const handleCreateGameClicked = () => {
		navigate('/create');
	};
	const handleJoinGameClicked = () => {
		navigate('/join');
	};
	return (
		<div>
			<h1>Home</h1>
			<button onClick={handleCreateGameClicked}>Create Game</button>
			<button onClick={handleJoinGameClicked}>Join Game</button>
		</div>
	);
}
