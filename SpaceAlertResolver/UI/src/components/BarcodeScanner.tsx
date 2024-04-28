import cx from 'classnames';
import styles from './BarcodeScanner.css';
import React, { useState, useEffect, useRef } from 'react';
import { useBarcodeScanning, useCamera } from '~/hooks';
import { Button } from '@mui/material';

export enum IWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Done,
	Error
}

export interface BarcodeScannerProps {
	onBarcodesScan(barcodes: DetectedBarcode[]): void;
}

export function BarcodeScanner({ onBarcodesScan }: BarcodeScannerProps) {
	const desiredBarcodeCountText = 12;
	const parsedDesiredBarcodeCount = +desiredBarcodeCountText;
	const desiredBarcodeCount = parsedDesiredBarcodeCount > 0 ? parsedDesiredBarcodeCount : null;
	const [workflowState, setWorkflowState] = useState(IWorkflowState.Initial);
	const canvasRef = useRef<HTMLCanvasElement>();
	const videoRef = useRef<HTMLVideoElement>();

	const {
		barcodes,
		detectedBarcodeCount,
		scan,
		scanTimeoutId,
		reset: resetBarcodeScanning
	} = useBarcodeScanning({
		canvasRef,
		desiredBarcodeCount,
		videoRef
	});

	const {
		startCamera,
		stopCamera,
		cameraStarted,
		error,
		reset: resetUseCamera
	} = useCamera({
		scan,
		videoRef
	});

	useEffect(() => {
		if (cameraStarted) {
			setWorkflowState(IWorkflowState.CameraStarted);
		}
	}, [cameraStarted]);

	useEffect(() => {
		if (error) {
			setWorkflowState(IWorkflowState.Error);
		}
	}, [error]);

	useEffect(() => {
		if (barcodes.length) {
			setWorkflowState(IWorkflowState.Done);
		}
	}, [barcodes]);

	useEffect(() => {
		switch (workflowState) {
			case IWorkflowState.CameraStarting:
				startCamera();
				break;
			case IWorkflowState.Done:
				stopCamera();
				if (scanTimeoutId) {
					window.clearTimeout(scanTimeoutId);
				}
				onBarcodesScan(barcodes);
				break;
		}
	}, [workflowState]);

	const handleStartCameraClicked = () => {
		resetUseCamera();
		resetBarcodeScanning();
		setWorkflowState(IWorkflowState.CameraStarting);
	};

	const handleStopCameraClicked = () => {
		setWorkflowState(IWorkflowState.Done);
	};

	const captureVideoClassName = cx(styles.video, {
		[styles.hiddenVideo]: workflowState !== IWorkflowState.CameraStarted
	});

	const canStartScanning =
		workflowState === IWorkflowState.Initial ||
		workflowState === IWorkflowState.Done ||
		workflowState === IWorkflowState.Error;

	return (
		<div>
			<Button
				variant='contained'
				onClick={handleStartCameraClicked}
				disabled={desiredBarcodeCount === null || !canStartScanning}>
				Start Camera
			</Button>
			<Button
				variant='contained'
				onClick={handleStopCameraClicked}
				disabled={workflowState !== IWorkflowState.CameraStarted}>
				Stop Camera
			</Button>
			<canvas hidden ref={canvasRef}></canvas>
			{workflowState === IWorkflowState.Error && (
				<>
					<div>Uh Oh! Something went wrong.</div>
					<div>{error}</div>
				</>
			)}
			<div>Detected Barcodes: {detectedBarcodeCount}</div>
			<div className={styles.videoWrapper}>
				<video playsInline className={captureVideoClassName} autoPlay muted ref={videoRef}></video>
			</div>
		</div>
	);
}
