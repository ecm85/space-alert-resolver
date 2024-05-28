import React from 'react';
import { PlayerColor } from '~/models';
import cx from 'classnames';
import styles from './PlayerBoard.css';

export interface PlayerBoardProps {
	barcodeData: string[];
	playerColor: PlayerColor;
}

const playerBoardsByColor: Record<PlayerColor, string> = {
	[PlayerColor.Blue]: 'Blue',
	[PlayerColor.Red]: 'Red',
	[PlayerColor.Green]: 'Green',
	[PlayerColor.Yellow]: 'Yellow',
	[PlayerColor.Purple]: 'Purple'
};

export function PlayerBoard({ playerColor, barcodeData }: PlayerBoardProps) {
	const matchingColor = playerBoardsByColor[playerColor];
	const hasSelectedColor = matchingColor != null;
	const displayColor = hasSelectedColor ? matchingColor : 'Green';
	const topBoardImageClassName = cx(styles['top-board'], {
		[styles['missing-color']]: !hasSelectedColor
	});
	const bottomBoardImageClassName = cx(styles['bottom-board'], {
		[styles['missing-color']]: !hasSelectedColor
	});
	return (
		<div className={styles['root']}>
			<div className={styles['board-wrapper']}>
				<img className={topBoardImageClassName} src={`\\Images\\Boards\\${displayColor}-1.png`} />
				<div className={styles['cards-wrapper']}>
					{barcodeData.slice(0, 3).map(scannedCard => (
						<div className={styles['card-wrapper']}>
							<img className={styles['card']} src={`\\Images\\Cards\\${scannedCard}.png`} />
						</div>
					))}
				</div>
				<div className={styles['cards-wrapper']}>
					{barcodeData.slice(3, 7).map(scannedCard => (
						<div className={styles['card-wrapper']}>
							<img className={styles['card']} src={`\\Images\\Cards\\${scannedCard}.png`} />
						</div>
					))}
				</div>
			</div>
			<div className={styles['board-wrapper']}>
				<img
					className={bottomBoardImageClassName}
					src={`\\Images\\Boards\\${displayColor}-2.png`}
				/>
				<div className={styles['cards-wrapper']}>
					{barcodeData.slice(7, 12).map(scannedCard => (
						<div className={styles['card-wrapper']}>
							<img className={styles['card']} src={`\\Images\\Cards\\${scannedCard}.png`} />
						</div>
					))}
				</div>
			</div>
		</div>
	);
}
