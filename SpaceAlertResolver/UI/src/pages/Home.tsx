import { Card, CardActionArea, CardContent, Typography } from '@mui/material';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import styles from './Home.css';

export function Home() {
	const navigate = useNavigate();

	const handleCreateGameClicked = () => {
		navigate('/create');
	};
	const handleJoinGameClicked = () => {
		navigate('/client');
	};
	return (
		<div>
			<h1>Home</h1>
			<div className={styles['card-row']}>
				<Card className={styles['card']}>
					<CardActionArea onClick={handleCreateGameClicked}>
						<CardContent>
							<Typography gutterBottom variant='h5' component='div'>
								Create Game
							</Typography>
							<Typography variant='body2' color='text.secondary'>
								Start a new game. You will receive a game code to allow other players to send you
								their card choices (or you can just just enter them yourself).
							</Typography>
						</CardContent>
					</CardActionArea>
				</Card>
				<Card className={styles['card']}>
					<CardActionArea onClick={handleJoinGameClicked}>
						<CardContent>
							<Typography gutterBottom variant='h5' component='div'>
								Join Game
							</Typography>
							<Typography variant='body2' color='text.secondary'>
								Use this option to send your card choices to the host. You will need the game code
								from the host.
							</Typography>
						</CardContent>
					</CardActionArea>
				</Card>
			</div>
		</div>
	);
}
