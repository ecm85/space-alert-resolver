import styles from './BarcodeScanningWorkflow.module.css';
import { useState, useRef, useCallback, MutableRefObject, useEffect } from 'react';
import Typography from '@mui/material/Typography/Typography';
import { BarcodeScanner } from './BarcodeScanner';
import { useCardScanning } from '~/hooks/useCardScanning';
import { Button } from '@mui/material';
import { DetectedBarcode } from 'barcode-detector';
import { BarcodeScanningWorkflowState as WorkflowState } from '~/models';
import { useBarcodeValidation } from '~/hooks/useBarcodeValidation';

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
	const [lastScanResult, setLastScanResult] = useState({
		barcodesInOrder: [] as DetectedBarcode[],
		dividingLine: null as number | null,
	});
	const { getBarcodesInOrder, drawDetectedBarcodes } = useCardScanning();

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
		(newBarcodes: DetectedBarcode[], videoRef: MutableRefObject<HTMLVideoElement | null>) => {
			if (barcodeCanvasRef.current === null) {
				throw new Error('Barcode canvas was null somehow');
			}
			if (videoRef.current === null) {
				throw new Error('Video was null somehow');
			}
			const { barcodesInOrder, dividingLine } = getBarcodesInOrder(newBarcodes);
			const barcodeCanvas = barcodeCanvasRef.current.getContext('2d', {
				willReadFrequently: true,
			});
			if (barcodeCanvas === null) {
				throw new Error('Barcode canvas context was null somehow');
			}
			const { videoHeight, videoWidth } = videoRef.current;
			barcodeCanvasRef.current.height = videoHeight;
			barcodeCanvasRef.current.width = videoWidth;
			drawDetectedBarcodes(barcodesInOrder, dividingLine, barcodeCanvas);
			setLastScanResult({ barcodesInOrder, dividingLine });
		},
		[getBarcodesInOrder, drawDetectedBarcodes],
	);

	const { hasEnoughBarcodes, dividingLineExists, dividingLineIsValid } = useBarcodeValidation({
		...lastScanResult,
	});

	const isValid = hasEnoughBarcodes && dividingLineExists && dividingLineIsValid;

	useEffect(() => {
		if (isValid) {
			onBarcodesScan(lastScanResult.barcodesInOrder);
		}
	}, [isValid, onBarcodesScan, lastScanResult.barcodesInOrder]);

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
					onBarcodesScan={handleBarcodesScanned}
					onCameraStart={handleCameraStarted}
					onError={handleError}
				/>
			)}
			<div className={styles['canvas-wrapper']}>
				<canvas className={styles['canvas']} ref={cameraCanvasRef}></canvas>
				<canvas className={styles['canvas-overlay']} ref={barcodeCanvasRef}></canvas>
			</div>
			{isScanning &&
				(!hasEnoughBarcodes ? (
					<Typography variant="body1">
						Looking for 12 barcodes. Found: {lastScanResult.barcodesInOrder.length}.
					</Typography>
				) : !dividingLineExists ? (
					<Typography variant="body1">Ensure that the bottom row is below the top row.</Typography>
				) : !dividingLineIsValid ? (
					<Typography variant="body1">
						Ensure the dividing line separates the rows clearly.
					</Typography>
				) : (
					<></>
				))}
		</div>
	);
}
