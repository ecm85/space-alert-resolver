import { SplitCard, WholeCard } from '~/models';
import { SplitPlayerCard } from './SplitPlayerCard';
import { WholePlayerCard } from './WholePlayerCard';

export interface PlayerCardProps {
	scannedCard: WholeCard | SplitCard;
	hideCard?: boolean;
}

export function PlayerCard({ scannedCard, hideCard }: PlayerCardProps) {
	return (
		<>
			{scannedCard.type === 'split' && (
				<SplitPlayerCard hideCard={hideCard} scannedCard={scannedCard} />
			)}
			{scannedCard.type === 'whole' && (
				<WholePlayerCard hideCard={hideCard} scannedCard={scannedCard} />
			)}
		</>
	);
}
