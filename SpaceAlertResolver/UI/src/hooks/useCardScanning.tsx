import { DetectedBarcode } from 'barcode-detector';
import { useCallback } from 'react';

export function useCardScanning() {
	const getBarcodesInOrder = useCallback((barcodes: DetectedBarcode[]) => {
		if (barcodes.length < 2) {
			return { barcodesInOrder: barcodes, dividingLine: null };
		}
		const sortedByX = sortByX(barcodes);
		const dividingLine = getDividingLine(sortedByX[0], sortedByX[1]);
		if (dividingLine === null) {
			return { barcodesInOrder: barcodes, dividingLine: null };
		}
		const top = [];
		const bottom = [];
		for (const barcode of barcodes) {
			if (barcode.boundingBox.bottom < dividingLine) {
				top.push(barcode);
			} else {
				bottom.push(barcode);
			}
		}
		return { barcodesInOrder: [...sortByX(top), ...sortByX(bottom)], dividingLine };
	}, []);

	const sortByX = (barcodes: DetectedBarcode[]) => {
		return [...barcodes].sort((first, second) => first.boundingBox.left - second.boundingBox.left);
	};

	const getDividingLine = (first: DetectedBarcode, second: DetectedBarcode) => {
		const upperCode = first.boundingBox.bottom > second.boundingBox.bottom ? second : first;
		const lowerCode = upperCode === first ? second : first;
		const upperCodeCenter = upperCode.boundingBox.left + upperCode.boundingBox.width / 2;
		const isYAligned =
			upperCodeCenter > lowerCode.boundingBox.left && upperCodeCenter < lowerCode.boundingBox.right;
		if (!isYAligned) {
			return null;
		}
		return (
			upperCode.boundingBox.bottom + (lowerCode.boundingBox.top - upperCode.boundingBox.bottom) / 2
		);
	};

	const drawDetectedBarcodes = useCallback(
		(
			barcodesInOrder: DetectedBarcode[],
			dividingLine: number | null,
			canvas: CanvasRenderingContext2D,
		) => {
			// TODO: Show line between columns?
			// TODO: Update colors
			const dividingLineWidth = 3;
			if (dividingLine) {
				canvas.fillStyle = 'blue';
				canvas.fillRect(0, dividingLine, canvas.canvas.width, dividingLineWidth);
			}
			let index = 1;
			for (const barcode of barcodesInOrder) {
				canvas.fillStyle = 'blue';
				canvas.fillRect(
					barcode.boundingBox.left,
					barcode.boundingBox.top,
					barcode.boundingBox.width,
					barcode.boundingBox.height,
				);
				canvas.font = '25px monospace';
				canvas.fillStyle = 'red';
				canvas.fillText(index.toString(), barcode.boundingBox.left, barcode.boundingBox.bottom);
				index++;
			}
		},
		[],
	);

	return {
		getBarcodesInOrder,
		drawDetectedBarcodes,
	};
}
