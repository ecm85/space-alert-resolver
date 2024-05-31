import { Orientation } from '~/models';
import { DetectedBarcode } from 'barcode-detector';

export function useBarcodeOrientation() {
	const getOrientation = (barcode: DetectedBarcode) => {
		const [trueUpperLeft, trueUpperRight, trueLowerRight, trueLowerLeft] = barcode.cornerPoints;
		if (
			trueUpperLeft.x < trueUpperRight.x &&
			trueLowerLeft.x < trueLowerRight.x &&
			trueUpperLeft.y < trueLowerLeft.y &&
			trueUpperRight.y < trueLowerRight.y
		)
			return Orientation.Top;
		if (
			trueUpperLeft.x > trueUpperRight.x &&
			trueLowerLeft.x > trueLowerRight.x &&
			trueUpperLeft.y > trueLowerLeft.y &&
			trueUpperRight.y > trueLowerRight.y
		)
			return Orientation.Bottom;
		return Orientation.Other;
	};
	return { getOrientation };
}
