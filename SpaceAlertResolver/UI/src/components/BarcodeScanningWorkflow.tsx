import styles from './BarcodeScanningWorkflow.module.css';
import { useState, useRef, useCallback } from 'react';
import Typography from '@mui/material/Typography/Typography';
import { BarcodeScanner } from './BarcodeScanner';
import { useCardScanning } from '~/hooks/useCardScanning';
import { Button } from '@mui/material';
import { DetectedBarcode } from 'barcode-detector';
import { BarcodeScanningWorkflowState as WorkflowState } from '~/models';

export interface BarcodeScanningWorkflowProps {
	onBarcodesScan(barcodes: DetectedBarcode[]): void;
}

export function BarcodeScanningWorkflow({ onBarcodesScan }: BarcodeScanningWorkflowProps) {
	const [workflowState, setWorkflowState] = useState(WorkflowState.Initial);
	const cameraCanvasRef = useRef<HTMLCanvasElement>(null);
	const barcodeCanvasRef = useRef<HTMLCanvasElement>(null);
	const [error, setError] = useState<string | null>(null);
	const isScanning =
		workflowState === WorkflowState.CameraStarting || workflowState === WorkflowState.CameraStarted;
	const { getBarcodesInOrder, drawDetectedBarcodes, validateBarcodes } = useCardScanning();

	const handleError = useCallback(
		(newError: string) => {
			setWorkflowState(WorkflowState.Error);
			setError(newError);
		},
		[setWorkflowState, setError],
	);

	const handleCameraStarted = useCallback(() => {
		setWorkflowState(WorkflowState.CameraStarted);
	}, [setWorkflowState]);

	const handleBarcodesScanned = useCallback(
		(newBarcodes: DetectedBarcode[], canvas: CanvasRenderingContext2D) => {
			const { barcodesInOrder, dividingLine } = getBarcodesInOrder(newBarcodes);
			drawDetectedBarcodes(barcodesInOrder, dividingLine, canvas);
			const errors = validateBarcodes(barcodesInOrder, dividingLine);
			if (!errors) {
				onBarcodesScan(barcodesInOrder);
			}
			return errors;
		},
		[getBarcodesInOrder, drawDetectedBarcodes, validateBarcodes, onBarcodesScan],
	);

	const handleScanClicked = () => {
		setWorkflowState(WorkflowState.CameraStarting);
	};

	const handleStopCameraClicked = () => {
		setWorkflowState(WorkflowState.Initial);
	};

	return (
		<div className={styles['root']}>
			<div className={styles['buttons']}>
				<Button variant="contained" onClick={handleScanClicked} disabled={isScanning}>
					Start Camera
				</Button>
				<Button
					variant="contained"
					onClick={handleStopCameraClicked}
					disabled={workflowState !== WorkflowState.CameraStarted}
				>
					Stop Camera
				</Button>
			</div>
			{/* TODO: Show help */}
			{workflowState === WorkflowState.CameraStarting && (
				<Typography variant="body1">Starting camera...</Typography>
			)}
			{workflowState === WorkflowState.Error && (
				<>
					<Typography variant="body1">Uh Oh! Something went wrong.</Typography>
					<Typography variant="body1">{error}</Typography>
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
			<div className={styles['canvas-wrapper']}>
				<canvas className={styles['canvas']} ref={cameraCanvasRef}></canvas>
				<canvas className={styles['canvas-overlay']} ref={barcodeCanvasRef}></canvas>
			</div>
		</div>
	);
}
