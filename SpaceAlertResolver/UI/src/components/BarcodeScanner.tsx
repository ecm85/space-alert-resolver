import styles from './BarcodeScanner.css';
import React, { useState, useEffect, useRef, MutableRefObject } from 'react';
import { useBarcodeScanning, useCamera } from '~/hooks';
import { Button } from '@mui/material';
import Typography from '@mui/material/Typography/Typography';

export enum IWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Done,
	Error
}

export interface BarcodeScannerProps {
	validateBarcodes(barcodes: DetectedBarcode[]): React.ReactNode;
	onBarcodesScan(barcodes: DetectedBarcode[]): void;
	drawDetectedBarcodes(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): void;
	canvasRef: MutableRefObject<HTMLCanvasElement>;
}

export function BarcodeScanner({
	onBarcodesScan,
	validateBarcodes,
	drawDetectedBarcodes,
	canvasRef
}: BarcodeScannerProps) {
	const [workflowState, setWorkflowState] = useState(IWorkflowState.Initial);
	const videoRef = useRef<HTMLVideoElement>();

	const {
		barcodes,
		validationError,
		scan,
		cancelScanning,
		reset: resetBarcodeScanning
	} = useBarcodeScanning({
		canvasRef,
		validateBarcodes,
		videoRef,
		drawDetectedBarcodes
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
			case IWorkflowState.Initial:
				stopCamera();
				cancelScanning();
				break;
			case IWorkflowState.CameraStarting:
				startCamera();
				break;
			case IWorkflowState.Done:
				stopCamera();
				cancelScanning();
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
		setWorkflowState(IWorkflowState.Initial);
	};

	const canStartScanning =
		workflowState === IWorkflowState.Initial ||
		workflowState === IWorkflowState.Done ||
		workflowState === IWorkflowState.Error;

	return (
		<div className={styles['root']}>
			<div className={styles['buttons']}>
				<Button variant='contained' onClick={handleStartCameraClicked} disabled={!canStartScanning}>
					{workflowState === IWorkflowState.Initial ? 'Start Camera' : 'Scan Again'}
				</Button>
				<Button
					variant='contained'
					onClick={handleStopCameraClicked}
					disabled={workflowState !== IWorkflowState.CameraStarted}>
					Stop Camera
				</Button>
			</div>
			{workflowState === IWorkflowState.CameraStarting && (
				<Typography variant='body1'>Starting camera...</Typography>
			)}
			{workflowState === IWorkflowState.Error && (
				<>
					<Typography variant='body1'>Uh Oh! Something went wrong.</Typography>
					<Typography variant='body1'>{error}</Typography>
				</>
			)}
			{validationError}
			<video className={styles['video']} autoPlay muted ref={videoRef}></video>
		</div>
	);
}
