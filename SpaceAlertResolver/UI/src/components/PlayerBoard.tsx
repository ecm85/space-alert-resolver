import { ScannedCard, PlayerColor } from '~/models';
import cx from 'classnames';
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
	const displayColor = hasSelectedColor ? matchingColor : 'Green';
	const topBoardImageClassName = cx(styles['top-board'], {
		[styles['missing-color']]: !hasSelectedColor,
	});
	const bottomBoardImageClassName = cx(styles['bottom-board'], {
		[styles['missing-color']]: !hasSelectedColor,
	});
	return (
		<div className={styles['root']}>
			<div className={styles['board-wrapper']}>
				<img className={topBoardImageClassName} src={`\\Images\\Boards\\${displayColor}-1.png`} />
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
				<img
					className={bottomBoardImageClassName}
					src={`\\Images\\Boards\\${displayColor}-2.png`}
				/>
				<PlayerCards
					hideCards={hideCards}
					scannedCards={scannedCards.slice(7, 12)}
					wrapperClassName={styles['cards-wrapper-7']}
				/>
			</div>
		</div>
	);
}
