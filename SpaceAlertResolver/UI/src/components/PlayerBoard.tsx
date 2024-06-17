import { ScannedCard, PlayerColor } from '~/models';
import styles from './PlayerBoard.module.css';
import { PlayerCards } from './PlayerCards';

export interface PlayerBoardProps {
	scannedCards: ScannedCard[];
	playerColor: PlayerColor | null;
	hideCards?: boolean;
}

const playerBoardsByColor: Record<PlayerColor, string> = {
	[PlayerColor.Blue]: 'Blue',
	[PlayerColor.Red]: 'Red',
	[PlayerColor.Green]: 'Green',
	[PlayerColor.Yellow]: 'Yellow',
	[PlayerColor.Purple]: 'Purple',
};

export function PlayerBoard({ playerColor, scannedCards, hideCards }: PlayerBoardProps) {
	const matchingColor = playerColor == null ? null : playerBoardsByColor[playerColor];
	const hasSelectedColor = matchingColor != null;
	const displayColor = hasSelectedColor ? matchingColor : 'Grey';
	return (
		<div className={styles['root']}>
			<div className={styles['board-wrapper']}>
				<img className={styles['top-board']} src={`\\Images\\Boards\\${displayColor}-1.png`} />
				<PlayerCards
					hideCards={hideCards}
					scannedCards={scannedCards.slice(0, 3)}
					wrapperClassName={styles['cards-wrapper-1']}
				/>
				<PlayerCards
					hideCards={hideCards}
					scannedCards={scannedCards.slice(3, 7)}
					wrapperClassName={styles['cards-wrapper-4']}
				/>
			</div>
			<div className={styles['board-wrapper']}>
				<img className={styles['bottom-board']} src={`\\Images\\Boards\\${displayColor}-2.png`} />
				<PlayerCards
					hideCards={hideCards}
					scannedCards={scannedCards.slice(7, 12)}
					wrapperClassName={styles['cards-wrapper-7']}
				/>
			</div>
		</div>
	);
}
