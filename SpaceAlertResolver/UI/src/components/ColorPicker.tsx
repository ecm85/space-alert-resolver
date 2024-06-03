import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';
import PersonIcon from '@mui/icons-material/Person';
import { PlayerColor } from '~/models';
import styles from './ColorPicker.module.css';
import cx from 'classnames';

export interface ColorPickerProps {
	value: PlayerColor | null;
	onPickColor(color: PlayerColor): void;
}

export function ColorPicker({ value, onPickColor }: ColorPickerProps) {
	const allPlayerColors = [
		PlayerColor.Red,
		PlayerColor.Blue,
		PlayerColor.Green,
		PlayerColor.Yellow,
		PlayerColor.Purple,
	];
	const classNames: Record<PlayerColor, string> = {
		[PlayerColor.Red]: 'red',
		[PlayerColor.Blue]: 'blue',
		[PlayerColor.Green]: 'green',
		[PlayerColor.Yellow]: 'yellow',
		[PlayerColor.Purple]: 'purple',
	};
	const createColorPickedHandler = (playerColor: PlayerColor) => () => {
		onPickColor(playerColor);
	};
	return (
		<div className={styles['root']}>
			<ToggleButtonGroup value={value} size="small">
				{allPlayerColors.map((playerColor) => (
					<ToggleButton
						key={playerColor}
						value={playerColor}
						onClick={createColorPickedHandler(playerColor)}
					>
						<PersonIcon className={cx(styles['person-icon'], styles[classNames[playerColor]])} />
					</ToggleButton>
				))}
			</ToggleButtonGroup>
		</div>
	);
}
