import { MutableRefObject, useCallback } from 'react';
import { DetectedBarcode, BarcodeDetector } from 'barcode-detector';

export interface useBarcodeScanningProps {
	videoRef: MutableRefObject<HTMLVideoElement | null>;
	cameraCanvasRef: MutableRefObject<HTMLCanvasElement | null>;
	onBarcodesScan(
		barcodes: DetectedBarcode[],
		videoRef: MutableRefObject<HTMLVideoElement | null>,
	): void;
}

export const useBarcodeScanning = ({
	videoRef,
	cameraCanvasRef,
	onBarcodesScan,
}: useBarcodeScanningProps) => {
	const scan = useCallback(async () => {
		const barcodeDetector = new BarcodeDetector();
		const tryGetBarcodes = async () => {
			if (cameraCanvasRef.current == null || videoRef.current == null) {
				throw new Error('Unable to get canvas.');
			}
			const cameraCanvas = cameraCanvasRef.current.getContext('2d', {
				willReadFrequently: true,
			});
			if (cameraCanvas == null) {
				throw new Error('Unable to get canvas context');
			}
			const { videoWidth, videoHeight } = videoRef.current;

			const imageData = cameraCanvas.getImageData(0, 0, videoWidth, videoHeight);
			const barcodes = await barcodeDetector.detect(imageData);
			// TODO: Only accept barcodes in valid format (via a predicate passed in)
			// Valid = starts and ends with digit* / *digit, and is good orientation
			const validBarcodes = barcodes?.filter((barcode) => !!barcode.rawValue) ?? [];
			return onBarcodesScan(validBarcodes, videoRef);
		};
		console.log('scan');
		try {
			if (videoRef.current && videoRef.current.readyState === videoRef.current.HAVE_ENOUGH_DATA) {
				await tryGetBarcodes();
			}
		} catch (error) {
			if (error instanceof Error) {
				console.info(`unable to detect qr code: - ${error.message}`);
			}
			console.info(error);
		}
	}, [videoRef, cameraCanvasRef, onBarcodesScan]);
	return {
		scan,
	};
};
