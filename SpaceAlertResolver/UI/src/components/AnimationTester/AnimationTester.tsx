import Button from '@mui/material/Button';
import React, { useState } from 'react';
import { Widget } from './Widget';
import styles from './AnimationTester.css';
import { usePrevious } from '~/hooks/usePrevious';

export function AnimationTester() {
	const states = [
		{
			one: true,
			two: false,
			three: false
		},
		{
			one: false,
			two: true,
			three: false
		},
		{
			one: false,
			two: false,
			three: true
		},
		{
			one: false,
			two: true,
			three: false
		}
	];
	const [stateIndex, setStateIndex] = useState(0);
	const previousStateIndex = usePrevious(stateIndex);
	const widgetState = states[stateIndex];
	const previousState = previousStateIndex != null ? states[previousStateIndex] : null;
	const handleToggleClicked = () => {
		setStateIndex(stateIndex < states.length - 1 ? stateIndex + 1 : 0);
	};

	return (
		<div className={styles['root']}>
			<Widget isOn={widgetState.one} previousRight={previousState?.two} />
			<Widget
				isOn={widgetState.two}
				previousLeft={previousState?.one}
				previousRight={previousState?.three}
			/>
			<Widget isOn={widgetState.three} previousLeft={previousState?.two} />
			<Button className={styles['button']} variant='contained' onClick={handleToggleClicked}>
				Toggle
			</Button>
		</div>
	);
}
