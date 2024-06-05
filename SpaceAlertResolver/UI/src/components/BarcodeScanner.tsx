import { MutableRefObject, ReactNode, useCallback, useEffect, useRef } from 'react';
import { useBarcodeScanning, useCamera } from '~/hooks';
import styles from './BarcodeScanner.module.css';
import { DetectedBarcode } from 'barcode-detector';

export interface BarcodeScannerProps {
	cameraCanvasRef: MutableRefObject<HTMLCanvasElement | null>;
	barcodeCanvasRef: MutableRefObject<HTMLCanvasElement | null>;
	onCameraStart(): void;
	onBarcodesScan(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): ReactNode;
	onError(error: string): void;
}

const cameraTimeout = 10;
const canvasTimeout = 100;

export function BarcodeScanner({
	onCameraStart,
	onError,
	cameraCanvasRef,
	barcodeCanvasRef,
	onBarcodesScan,
}: BarcodeScannerProps) {
	const cameraIntervalIdRef = useRef<number | null>(null);
	const canvasIntervalIdRef = useRef<number | null>(null);
	const videoRef = useRef<HTMLVideoElement | null>(null);
	const { validationError, scan } = useBarcodeScanning({
		barcodeCanvasRef,
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
			const success = await scan();
			if (success) {
				window.clearInterval(canvasIntervalId);
			}
		}, canvasTimeout);
		canvasIntervalIdRef.current = canvasIntervalId;
		const cameraIntervalId = window.setInterval(drawCameraOnCanvas, cameraTimeout);
		cameraIntervalIdRef.current = cameraIntervalId;
	}, [cameraCanvasRef, scan]);

	const { startCamera, stopCamera, cameraStartedRef } = useCamera({
		processCamera,
		videoRef,
		onError,
	});

	useEffect(() => {
		if (videoRef.current) {
			startCamera();
		}
	}, [videoRef, startCamera]);

	useEffect(() => {
		if (cameraStartedRef.current) {
			onCameraStart();
		}
	}, [cameraStartedRef, onCameraStart]);

	useEffect(() => {
		const cameraStartedRefCurrent = cameraStartedRef.current;
		const cameraIntervalIdRefCurrent = cameraIntervalIdRef.current;
		const canvasIntervalIdRefCurrent = canvasIntervalIdRef.current;
		return () => {
			if (cameraStartedRefCurrent) {
				stopCamera();
			}
			if (cameraIntervalIdRefCurrent) {
				window.clearInterval(cameraIntervalIdRefCurrent);
			}
			if (canvasIntervalIdRefCurrent) {
				window.clearInterval(canvasIntervalIdRefCurrent);
			}
		};
	}, [cameraStartedRef, cameraIntervalIdRef, canvasIntervalIdRef, stopCamera]);

	return (
		<>
			{validationError}
			<video className={styles['video']} autoPlay playsInline muted ref={videoRef}></video>
		</>
	);
}
