import { ReactNode, useState, MutableRefObject } from 'react';
import { DetectedBarcode, BarcodeDetector } from 'barcode-detector';

export interface useBarcodeScanningProps {
	videoRef: MutableRefObject<HTMLVideoElement | null>;
	cameraCanvasRef: MutableRefObject<HTMLCanvasElement | null>;
	barcodeCanvasRef: MutableRefObject<HTMLCanvasElement | null>;
	onBarcodesScan(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): ReactNode;
}

export const useBarcodeScanning = ({
	videoRef,
	cameraCanvasRef,
	barcodeCanvasRef,
	onBarcodesScan,
}: useBarcodeScanningProps) => {
	const barcodeDetector = new BarcodeDetector();
	const [validationError, setValidationError] = useState<ReactNode>([]);
	const scan = async () => {
		console.log('scan');
		try {
			if (videoRef.current && videoRef.current.readyState === videoRef.current.HAVE_ENOUGH_DATA) {
				const newValidationError = await tryGetBarcodes();
				setValidationError(newValidationError);
				return !newValidationError;
			} else {
				return false;
			}
		} catch (error) {
			if (error instanceof Error) {
				console.info(`unable to detect qr code: - ${error.message}`);
			}
			console.info(error);
		}
	};
	const tryGetBarcodes = async () => {
		if (
			cameraCanvasRef.current == null ||
			barcodeCanvasRef.current == null ||
			videoRef.current == null
		) {
			throw new Error('Unable to get canvas.');
		}
		const cameraCanvas = cameraCanvasRef.current.getContext('2d', {
			willReadFrequently: true,
		});
		const barcodeCanvas = barcodeCanvasRef.current.getContext('2d', {
			willReadFrequently: true,
		});
		if (cameraCanvas == null || barcodeCanvas == null) {
			throw new Error('Unable to get canvas context');
		}
		const { videoWidth, videoHeight } = videoRef.current;
		barcodeCanvasRef.current.height = videoHeight;
		barcodeCanvasRef.current.width = videoWidth;
		const imageData = cameraCanvas.getImageData(0, 0, videoWidth, videoHeight);
		const barcodes = await barcodeDetector.detect(imageData);
		// TODO: Only accept barcodes in valid format (via a predicate passed in)
		// Valid = starts and ends with digit* / *digit, and is good orientation
		const validBarcodes = barcodes?.filter((barcode) => !!barcode.rawValue) ?? [];
		return onBarcodesScan(validBarcodes, barcodeCanvas);
	};

	return {
		scan,
		validationError,
	};
};
