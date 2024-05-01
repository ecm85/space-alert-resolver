import { ReactNode, useState } from 'react';
import { BarcodeDetector } from 'barcode-detector';

export interface useBarcodeScanningProps {
	videoRef: React.MutableRefObject<HTMLVideoElement>;
	canvasRef: React.MutableRefObject<HTMLCanvasElement>;
	validateBarcodes(barcodes: DetectedBarcode[]): ReactNode;
	drawDetectedBarcodes(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): void;
}

const timeout = 50;

export const useBarcodeScanning = ({
	videoRef,
	canvasRef,
	validateBarcodes,
	drawDetectedBarcodes
}: useBarcodeScanningProps) => {
	const initialBarcodes: DetectedBarcode[] = [];
	const barcodeDetector = new BarcodeDetector();
	const [barcodes, setBarcodes] = useState(initialBarcodes);
	const [scanTimeoutId, setScanTimeoutId] = useState<number>(null);
	const [validationError, setValidationError] = useState<ReactNode>([]);
	const scan = async () => {
		try {
			if (videoRef.current.readyState === videoRef.current.HAVE_ENOUGH_DATA) {
				const barcodes = await tryGetBarcodes();
				const newValidationError = validateBarcodes(barcodes);
				setValidationError(newValidationError);
				if (!newValidationError) {
					setBarcodes(barcodes);
				} else {
					setScanTimeoutId(window.setTimeout(scan, timeout));
				}
			} else {
				setScanTimeoutId(window.setTimeout(scan, timeout));
			}
		} catch (error) {
			console.info(`unable to detect qr code: - ${error.message}`);
			console.info(error);
		}
	};
	const tryGetBarcodes = async () => {
		const canvas = canvasRef.current.getContext('2d', {
			willReadFrequently: true
		});
		const { videoWidth, videoHeight } = videoRef.current;
		canvasRef.current.height = videoHeight;
		canvasRef.current.width = videoWidth;
		canvas.drawImage(videoRef.current, 0, 0, videoWidth, videoHeight);
		const imageData = canvas.getImageData(0, 0, videoWidth, videoHeight);
		const barcodes = await barcodeDetector.detect(imageData);
		const validBarcodes = barcodes?.filter(barcode => !!barcode.rawValue) ?? [];
		drawDetectedBarcodes(validBarcodes, canvas);
		return validBarcodes;
	};
	const reset = () => {
		setBarcodes(initialBarcodes);
	};
	return {
		scan,
		barcodes,
		scanTimeoutId,
		reset,
		validationError
	};
};
