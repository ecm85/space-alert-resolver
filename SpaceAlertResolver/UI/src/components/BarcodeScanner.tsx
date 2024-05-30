import { MutableRefObject, ReactNode, useEffect, useRef } from 'react';
import { useBarcodeScanning, useCamera } from '~/hooks';
import styles from './BarcodeScanner.css';

export interface BarcodeScannerProps {
	cameraCanvasRef: MutableRefObject<HTMLCanvasElement>;
	barcodeCanvasRef: MutableRefObject<HTMLCanvasElement>;
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
	const cameraIntervalIdRef = useRef<number>(null);
	const canvasIntervalIdRef = useRef<number>(null);
	const videoRef = useRef<HTMLVideoElement>();
	const { validationError, scan } = useBarcodeScanning({
		barcodeCanvasRef,
		cameraCanvasRef,
		videoRef,
		onBarcodesScan,
	});

	const processCamera = () => {
		const canvasIntervalId = window.setInterval(async () => {
			const success = await scan();
			if (success) {
				window.clearInterval(canvasIntervalId);
			}
		}, canvasTimeout);
		canvasIntervalIdRef.current = canvasIntervalId;
		const cameraIntervalId = window.setInterval(drawCameraOnCanvas, cameraTimeout);
		cameraIntervalIdRef.current = cameraIntervalId;
	};

	const { startCamera, stopCamera, cameraStartedRef, drawCameraOnCanvas } = useCamera({
		processCamera,
		videoRef,
		onError,
		canvasRef: cameraCanvasRef,
	});

	useEffect(() => {
		if (videoRef.current) {
			startCamera();
		}
	}, [videoRef]);

	useEffect(() => {
		if (cameraStartedRef.current) {
			onCameraStart();
		}
	}, [cameraStartedRef.current]);

	useEffect(() => {
		return () => {
			if (cameraStartedRef.current) {
				stopCamera();
			}
			if (cameraIntervalIdRef.current) {
				window.clearInterval(cameraIntervalIdRef.current);
			}
			if (canvasIntervalIdRef.current) {
				window.clearInterval(canvasIntervalIdRef.current);
			}
		};
	}, []);

	return (
		<>
			{validationError}
			<video
				className={styles['video']}
				autoPlay
				webkit-playsinline
				playsInline
				muted
				ref={videoRef}
			></video>
		</>
	);
}
