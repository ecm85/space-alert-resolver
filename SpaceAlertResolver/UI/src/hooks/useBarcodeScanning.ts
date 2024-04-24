import { useState } from 'react';
import { BarcodeDetector } from 'barcode-detector';

export interface useBarcodeScanningProps {
	desiredBarcodeCount: number;
	videoRef: React.MutableRefObject<HTMLVideoElement>;
	canvasRef: React.MutableRefObject<HTMLCanvasElement>;
}

export const useBarcodeScanning = ({
	desiredBarcodeCount,
	videoRef,
	canvasRef
}: useBarcodeScanningProps) => {
	const initialBarcodes: DetectedBarcode[] = [];
	const barcodeDetector = new BarcodeDetector();
	const [detectedBarcodeCount, setDetectedBarcodeCount] = useState<number>(0);
	const [barcodes, setBarcodes] = useState(initialBarcodes);
	const [scanTimeoutId, setScanTimeoutId] = useState<number>(null);
	const scan = async () => {
		try {
			const barcodes = await tryGetBarcodes();
			const validBarcodes = barcodes?.filter(barcode => !!barcode.rawValue) ?? [];
			setDetectedBarcodeCount(validBarcodes.length);
			if (validBarcodes?.length == desiredBarcodeCount) {
				setBarcodes(barcodes);
			} else {
				setScanTimeoutId(window.setTimeout(scan, 250));
			}
		} catch (error) {
			console.info(`unable to detect qr code: - ${error.message}`);
		}
	};
	const tryGetBarcodes = () => {
		if (videoRef.current.readyState !== videoRef.current.HAVE_ENOUGH_DATA) {
			return null;
		}
		const canvas = canvasRef.current.getContext('2d');
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
		detectedBarcodeCount,
		barcodes,
		scanTimeoutId,
		reset
	};
};
