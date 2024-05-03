import React, { MutableRefObject, ReactNode, useEffect, useRef } from 'react';
import { useBarcodeScanning, useCamera } from '~/hooks';
import styles from './BarcodeScanner.css';

export interface BarcodeScannerProps {
	canvasRef: MutableRefObject<HTMLCanvasElement>;
	onCameraStart(): void;
	onBarcodesScan(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): ReactNode;
	onError(error: string): void;
}

export function BarcodeScanner({
	onCameraStart,
	onError,
	canvasRef,
	onBarcodesScan
}: BarcodeScannerProps) {
	const videoRef = useRef<HTMLVideoElement>();
	const { validationError, scan } = useBarcodeScanning({
		canvasRef,
		videoRef,
		onBarcodesScan
	});

	const { startCamera, cameraStarted, error } = useCamera({
		scan,
		videoRef
	});

	useEffect(() => {
		startCamera();
	}, []);

	useEffect(() => {
		if (cameraStarted) {
			onCameraStart();
		}
	}, [cameraStarted]);

	useEffect(() => {
		if (error) {
			onError(error);
		}
	}, [error]);

	return (
		<>
			{validationError}
			<video className={styles['video']} autoPlay muted ref={videoRef}></video>
		</>
	);
}
