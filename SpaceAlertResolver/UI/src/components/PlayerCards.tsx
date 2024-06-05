import cx from 'classnames';
import styles from './PlayerCards.module.css';
import { ParsedBarcode } from '~/models';

export interface PlayerCardsProps {
	wrapperClassName: string;
	barcodeData: ParsedBarcode[];
}

export function PlayerCards({ wrapperClassName, barcodeData }: PlayerCardsProps) {
	const className = cx(wrapperClassName, styles['cards-wrapper']);
	return (
		<div className={className}>
			{barcodeData.map((scannedCard, index) => (
				<div key={index} className={styles['card-wrapper']}>
					{scannedCard != null && (
						<img className={styles['card']} src={`\\Images\\Cards\\${scannedCard}.png`} />
					)}
				</div>
			))}
		</div>
	);
}
