import { WholeCard } from '~/models';
import styles from './WholePlayerCard.module.css';

export interface WholePlayerCardProps {
	hideCard?: boolean;
	scannedCard: WholeCard;
}

export function WholePlayerCard({ hideCard, scannedCard }: WholePlayerCardProps) {
	return hideCard ? (
		<img className={styles['card-back']} src={`\\Images\\Cards\\${scannedCard.back}.png`} />
	) : (
		<img className={styles['card-front']} src={`\\Images\\Cards\\${scannedCard.front}.png`} />
	);
}
