import { Orientation } from '~/models';

export function useBarcodeOrientation() {
	const getOrientation = (barcode: DetectedBarcode) => {
		const {
			cornerPoints: [trueUpperLeft],
			boundingBox
		} = barcode;
		if (trueUpperLeft.x === boundingBox.left && trueUpperLeft.y === boundingBox.top)
			return Orientation.Top;
		if (trueUpperLeft.x === boundingBox.right && trueUpperLeft.y === boundingBox.bottom)
			return Orientation.Bottom;
		return Orientation.Other;
	};
	return { getOrientation };
}
