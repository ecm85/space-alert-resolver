import styles from './BarcodeScanningWorkflow.css';
import React, { useState, MutableRefObject, ReactNode } from 'react';
import { Button } from '@mui/material';
import Typography from '@mui/material/Typography/Typography';
import { BarcodeScanner } from './BarcodeScanner';

export enum IWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Done,
	Error
}

export interface BarcodeScanningWorkflowProps {
	onBarcodesScan(barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D): ReactNode;
	onClear(): void;
	cameraCanvasRef: MutableRefObject<HTMLCanvasElement>;
	barcodeCanvasRef: MutableRefObject<HTMLCanvasElement>;
}

export function BarcodeScanningWorkflow({
	onBarcodesScan,
	onClear,
	cameraCanvasRef,
	barcodeCanvasRef
}: BarcodeScanningWorkflowProps) {
	const [workflowState, setWorkflowState] = useState(IWorkflowState.Initial);
	const [error, setError] = useState(null);
	const isScanning =
		workflowState === IWorkflowState.CameraStarting ||
		workflowState === IWorkflowState.CameraStarted;

	const handleError = (newError: string) => {
		setWorkflowState(IWorkflowState.Error);
		setError(newError);
	};

	const handleCameraStarted = () => {
		setWorkflowState(IWorkflowState.CameraStarted);
	};

	const handleStartCameraClicked = () => {
		onClear();
		setWorkflowState(IWorkflowState.CameraStarting);
	};

	const handleStopCameraClicked = () => {
		onClear();
		setWorkflowState(IWorkflowState.Initial);
	};

	const handleBarcodesScanned = (barcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D) => {
		const result = onBarcodesScan(barcodes, canvas);
		if (!result) {
			setWorkflowState(IWorkflowState.Done);
		}
		return result;
	};

	return (
		<div className={styles['root']}>
			<div className={styles['buttons']}>
				<Button variant='contained' onClick={handleStartCameraClicked} disabled={isScanning}>
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
			{isScanning && (
				<BarcodeScanner
					cameraCanvasRef={cameraCanvasRef}
					barcodeCanvasRef={barcodeCanvasRef}
					onBarcodesScan={handleBarcodesScanned}
					onCameraStart={handleCameraStarted}
					onError={handleError}
				/>
			)}
		</div>
	);
}
