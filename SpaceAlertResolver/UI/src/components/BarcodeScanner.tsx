import { MutableRefObject, useCallback, useEffect, useRef } from 'react';
import { useBarcodeScanning, useCamera } from '~/hooks';
import styles from './BarcodeScanner.module.css';
import { DetectedBarcode } from 'barcode-detector';

export interface BarcodeScannerProps {
	cameraCanvasRef: MutableRefObject<HTMLCanvasElement | null>;
	onCameraStart(): void;
	onBarcodesScan(
		barcodes: DetectedBarcode[],
		videoRef: MutableRefObject<HTMLVideoElement | null>,
	): void;
	onError(error: string): void;
}

const cameraTimeout = 10;
const canvasTimeout = 100;

export function BarcodeScanner({
	onCameraStart,
	onError,
	cameraCanvasRef,
	onBarcodesScan,
}: BarcodeScannerProps) {
	const cameraIntervalIdRef = useRef<number | null>(null);
	const canvasIntervalIdRef = useRef<number | null>(null);
	const videoRef = useRef<HTMLVideoElement | null>(null);
	const { scan } = useBarcodeScanning({
		cameraCanvasRef,
		videoRef,
		onBarcodesScan,
	});

	const processCamera = useCallback(() => {
		const drawCameraOnCanvas = () => {
			if (cameraCanvasRef.current == null || videoRef.current == null) {
				return;
			}
			const canvas = cameraCanvasRef.current.getContext('2d', {
				willReadFrequently: true,
			});
			const { videoWidth, videoHeight } = videoRef.current;
			cameraCanvasRef.current.height = videoHeight;
			cameraCanvasRef.current.width = videoWidth;
			canvas?.drawImage(videoRef.current, 0, 0, videoWidth, videoHeight);
		};

		const canvasIntervalId = window.setInterval(async () => {
			await scan();
		}, canvasTimeout);
		canvasIntervalIdRef.current = canvasIntervalId;
		const cameraIntervalId = window.setInterval(drawCameraOnCanvas, cameraTimeout);
		cameraIntervalIdRef.current = cameraIntervalId;
	}, [cameraCanvasRef, scan]);

	const { startCamera, stopCamera } = useCamera({
		processCamera,
		videoRef,
		onError,
		onCameraStart,
	});

	useEffect(() => {
		startCamera();
		return () => {
			stopCamera();
			if (cameraIntervalIdRef.current) {
				window.clearInterval(cameraIntervalIdRef.current);
			}
			if (canvasIntervalIdRef.current) {
				window.clearInterval(canvasIntervalIdRef.current);
			}
		};
	}, [startCamera, stopCamera]);

	return (
		<>
			<video className={styles['video']} autoPlay playsInline muted ref={videoRef}></video>
		</>
	);
}
