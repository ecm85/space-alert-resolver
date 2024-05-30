import styles from './Widget.css';
import cx from 'classnames';

interface WidgetProps {
	isOn: boolean;
	previousLeft?: boolean;
	previousRight?: boolean;
}

export function Widget({ isOn, previousLeft, previousRight }: WidgetProps) {
	const animateFromLeft = isOn && previousLeft;
	const animateFromRight = isOn && previousRight;
	const thingClassName = cx(styles['thing'], {
		[styles['thing-from-left']]: animateFromLeft,
		[styles['thing-from-right']]: animateFromRight,
	});
	return <div className={styles['root']}>{isOn && <div className={thingClassName}></div>}</div>;
}
