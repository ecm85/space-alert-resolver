import { ReactNode, useEffect, useState } from 'react';
import { BarcodeDetector } from 'barcode-detector';

export interface useBarcodeScanningProps {
	videoRef: React.MutableRefObject<HTMLVideoElement>;
	canvasRef: React.MutableRefObject<HTMLCanvasElement>;
	onBarcodesScan(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): ReactNode;
}

const timeout = 50;

export const useBarcodeScanning = ({
	videoRef,
	canvasRef,
	onBarcodesScan
}: useBarcodeScanningProps) => {
	const barcodeDetector = new BarcodeDetector();
	const [timeoutId, setTimeoutId] = useState<number>(null);
	const [validationError, setValidationError] = useState<ReactNode>([]);
	const scan = async () => {
		try {
			if (videoRef.current.readyState === videoRef.current.HAVE_ENOUGH_DATA) {
				const newValidationError = await tryGetBarcodes();
				setValidationError(newValidationError);
				if (newValidationError) {
					setTimeoutId(window.setTimeout(scan, timeout));
				}
			} else {
				setTimeoutId(window.setTimeout(scan, timeout));
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
		return onBarcodesScan(validBarcodes, canvas);
	};
	useEffect(() => {
		return () => {
			if (timeoutId) {
				clearTimeout(timeoutId);
			}
		};
	}, []);
	return {
		scan,
		validationError
	};
};
