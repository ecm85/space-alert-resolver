import { Orientation, SplitCard } from '~/models';
import styles from './SplitPlayerCard.module.css';
import cx from 'classnames';

export interface SplitPlayerCardProps {
	hideCard?: boolean;
	scannedCard: SplitCard;
}

export function SplitPlayerCard({ hideCard, scannedCard }: SplitPlayerCardProps) {
	const upsideDownClass = {
		[styles['upside-down']]: scannedCard.orientation === Orientation.Bottom,
	} as const;

	return hideCard ? (
		<img
			className={cx(styles['card-back'], upsideDownClass)}
			src={`\\Images\\Cards\\${scannedCard.back}.png`}
		/>
	) : (
		<div className={cx(styles['split-card-pieces-wrapper'], upsideDownClass)}>
			<img className={styles['card-top']} src={`\\Images\\Cards\\${scannedCard.top}.png`} />
			<img className={styles['card-bottom']} src={`\\Images\\Cards\\${scannedCard.bottom}.png`} />
		</div>
	);
}
