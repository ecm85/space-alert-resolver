import cx from 'classnames';
import styles from './PlayerCards.module.css';
import { ScannedCard } from '~/models';
import { PlayerCard } from './PlayerCard';

export interface PlayerCardsProps {
	wrapperClassName: string;
	scannedCards: ScannedCard[];
	hideCards?: boolean;
}

export function PlayerCards({ wrapperClassName, scannedCards, hideCards }: PlayerCardsProps) {
	const className = cx(wrapperClassName, styles['cards-wrapper']);
	return (
		<div className={className}>
			{scannedCards.map((scannedCard, index) => (
				<div key={index} className={styles['card-wrapper']}>
					{scannedCard != null && <PlayerCard scannedCard={scannedCard} hideCard={hideCards} />}
				</div>
			))}
		</div>
	);
}
