import React from 'react';
import cx from 'classnames';
import styles from './PlayerCards.css';

export interface PlayerCardsProps {
	wrapperClassName: string;
	barcodeData: string[];
}

export function PlayerCards({ wrapperClassName, barcodeData }: PlayerCardsProps) {
	const className = cx(wrapperClassName, styles['cards-wrapper']);
	return (
		<div className={className}>
			{barcodeData.map(scannedCard => (
				<div className={styles['card-wrapper']}>
					<img className={styles['card']} src={`\\Images\\Cards\\${scannedCard}.png`} />
				</div>
			))}
		</div>
	);
}
