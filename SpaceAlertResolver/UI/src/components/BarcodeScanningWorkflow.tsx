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
	canvasRef: MutableRefObject<HTMLCanvasElement>;
}

export function BarcodeScanningWorkflow({
	onBarcodesScan,
	canvasRef
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
		setWorkflowState(IWorkflowState.CameraStarting);
	};

	const handleStopCameraClicked = () => {
		setWorkflowState(IWorkflowState.Initial);
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
					canvasRef={canvasRef}
					onBarcodesScan={onBarcodesScan}
					onCameraStart={handleCameraStarted}
					onError={handleError}
				/>
			)}
		</div>
	);
}
