import { ReactNode, useState } from 'react';
import { BarcodeDetector } from 'barcode-detector';

export interface useBarcodeScanningProps {
	videoRef: React.MutableRefObject<HTMLVideoElement>;
	canvasRef: React.MutableRefObject<HTMLCanvasElement>;
	validateBarcodes(barcodes: DetectedBarcode[]): ReactNode;
}

export const useBarcodeScanning = ({
	validateBarcodes,
	videoRef,
	canvasRef
}: useBarcodeScanningProps) => {
	const initialBarcodes: DetectedBarcode[] = [];
	const barcodeDetector = new BarcodeDetector();
	const [barcodes, setBarcodes] = useState(initialBarcodes);
	const [scanTimeoutId, setScanTimeoutId] = useState<number>(null);
	const [validationError, setValidationError] = useState<ReactNode>([]);
	const scan = async () => {
		try {
			const barcodes = await tryGetBarcodes();
			const validBarcodes = barcodes?.filter(barcode => !!barcode.rawValue) ?? [];
			const newValidationError = validateBarcodes(validBarcodes);
			setValidationError(newValidationError);
			if (!newValidationError) {
				setBarcodes(barcodes);
			} else {
				setScanTimeoutId(window.setTimeout(scan, 250));
			}
		} catch (error) {
			console.info(`unable to detect qr code: - ${error.message}`);
			console.info(error);
		}
	};
	const tryGetBarcodes = () => {
		if (videoRef.current.readyState !== videoRef.current.HAVE_ENOUGH_DATA) {
			return null;
		}
		const canvas = canvasRef.current.getContext('2d', {
			willReadFrequently: true
		});
		const { videoWidth, videoHeight } = videoRef.current;
		canvasRef.current.height = videoHeight;
		canvasRef.current.width = videoWidth;
		canvas.drawImage(videoRef.current, 0, 0, videoWidth, videoHeight);
		const imageData = canvas.getImageData(0, 0, videoWidth, videoHeight);
		return barcodeDetector.detect(imageData);
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
