import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';
import PersonIcon from '@mui/icons-material/Person';
import { PlayerColor } from '~/models';
import styles from './ColorPicker.module.css';

export interface ColorPickerProps {
	value: PlayerColor | null;
	onPickColor(color: PlayerColor): void;
}

export function ColorPicker({ onPickColor }: ColorPickerProps) {
	const allPlayerColors = [
		PlayerColor.Red,
		PlayerColor.Blue,
		PlayerColor.Green,
		PlayerColor.Yellow,
		PlayerColor.Purple,
	];
	const createColorPickedHandler = (playerColor: PlayerColor) => () => {
		onPickColor(playerColor);
	};
	return (
		<div>
			<ToggleButtonGroup>
				{allPlayerColors.map((playerColor) => (
					<ToggleButton value={playerColor} onClick={createColorPickedHandler(playerColor)}>
						<PersonIcon className={styles[playerColor]} />
					</ToggleButton>
				))}
			</ToggleButtonGroup>
		</div>
	);
}
