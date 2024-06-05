import { DetectedBarcode } from 'barcode-detector';

export interface useBarcodeValidationProps {
	barcodesInOrder: DetectedBarcode[];
	dividingLine: number | null;
}

const dividingLineIsValid = (barcodesInOrder: DetectedBarcode[], dividingLine: number) => {
	const firstRow = barcodesInOrder.slice(0, 7);
	const secondRow = barcodesInOrder.slice(7);
	return (
		!firstRow.some((barcode) => barcode.boundingBox.bottom > dividingLine) &&
		!secondRow.some((barcode) => barcode.boundingBox.top < dividingLine)
	);
};

export const useBarcodeValidation = ({
	barcodesInOrder,
	dividingLine,
}: useBarcodeValidationProps) => {
	const hasEnoughBarcodes = barcodesInOrder.length === 12;
	const dividingLineExists = dividingLine != null;
	return {
		hasEnoughBarcodes,
		dividingLineExists,
		dividingLineIsValid: dividingLineExists && dividingLineIsValid(barcodesInOrder, dividingLine),
	};
};
