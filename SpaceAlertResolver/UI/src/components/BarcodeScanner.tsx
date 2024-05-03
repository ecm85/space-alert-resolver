import React, { MutableRefObject, ReactNode, useEffect, useRef } from 'react';
import { useBarcodeScanning, useCamera } from '~/hooks';
import styles from './BarcodeScanner.css';

export interface BarcodeScannerProps {
	canvasRef: MutableRefObject<HTMLCanvasElement>;
	onCameraStart(): void;
	onBarcodesScan(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): ReactNode;
	onError(error: string): void;
}

const timeout = 50;

export function BarcodeScanner({
	onCameraStart,
	onError,
	canvasRef,
	onBarcodesScan
}: BarcodeScannerProps) {
	const intervalIdRef = useRef<number>(null);
	const videoRef = useRef<HTMLVideoElement>();
	const { validationError, scan } = useBarcodeScanning({
		canvasRef,
		videoRef,
		onBarcodesScan
	});

	const processCamera = async () => {
		const newIntervalId = window.setInterval(async () => {
			const success = await scan();
			if (success) {
				window.clearInterval(newIntervalId);
			}
		}, timeout);
		intervalIdRef.current = newIntervalId;
	};

	const { startCamera, stopCamera, cameraStartedRef } = useCamera({
		processCamera,
		videoRef,
		onError
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
			if (intervalIdRef.current) {
				window.clearInterval(intervalIdRef.current);
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
				playsinline
				muted
				ref={videoRef}></video>
		</>
	);
}
