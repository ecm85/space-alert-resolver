import { FormControlLabel, FormGroup, Switch } from '@mui/material';

export type HideCardsSwitchProps = {
	hideCards: boolean;
	onChangeHideCards(newHideCards: boolean): void;
};

export function HideCardsSwitch({ hideCards, onChangeHideCards }: HideCardsSwitchProps) {
	const control = <Switch checked={hideCards} onChange={() => onChangeHideCards(!hideCards)} />;
	return (
		<div>
			<FormGroup>
				<FormControlLabel control={control} label="Hide cards (reveal turn-by-turn)" />
			</FormGroup>
		</div>
	);
}
