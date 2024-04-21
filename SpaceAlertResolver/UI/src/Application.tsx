import React, { useState, useEffect, useRef } from 'react';
import cx from 'classnames';
import styles from './Application.css';
import { useCamera } from './useCamera';
import { useBarcodeScanning } from './useBarcodeScanning';

export enum IWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Done,
	Error
}

export default function Application() {
	const [desiredBarcodeCountText, setDesiredBarcodeCountText] = useState<string>('1');
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
		cameraLogs,
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
	});

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

	const handleDesiredBarcodeCountChanged = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newDesiredBarcodeCount = e.target.value;
		setDesiredBarcodeCountText(newDesiredBarcodeCount);
	};

	const canStartScanning =
		workflowState === IWorkflowState.Initial ||
		workflowState === IWorkflowState.Done ||
		workflowState === IWorkflowState.Error;

	return (
		<div>
			<div>
				<label># of barcodes to scan</label>
				<input
					disabled={!canStartScanning}
					type='number'
					onChange={handleDesiredBarcodeCountChanged}
					value={desiredBarcodeCountText}></input>
			</div>
			<button
				onClick={handleStartCameraClicked}
				disabled={desiredBarcodeCount === null || !canStartScanning}>
				Start Camera
			</button>
			<button
				onClick={handleStopCameraClicked}
				disabled={workflowState !== IWorkflowState.CameraStarted}>
				Stop Camera
			</button>
			<canvas hidden ref={canvasRef}></canvas>
			{workflowState === IWorkflowState.Error && (
				<>
					<div>Uh Oh! Something went wrong.</div>
					<div>{error}</div>
				</>
			)}
			{cameraLogs.length > 0 ? (
				<div>
					Camera Logs:{' '}
					{cameraLogs.map(log => (
						<div>{log}</div>
					))}
				</div>
			) : (
				<div>No camera logs.</div>
			)}
			<div>Detected Barcodes: {detectedBarcodeCount}</div>
			<div>
				{barcodes.length > 0 &&
					barcodes.map((barcode, index) => (
						<div>
							Barcode {index + 1}: {barcode.rawValue}
							{barcode.cornerPoints.map(cornerPoint => (
								<div>
									X: {cornerPoint.x}, Y: {cornerPoint.y}
								</div>
							))}
						</div>
					))}
			</div>
			<div className={styles.videoWrapper}>
				<video playsInline className={captureVideoClassName} autoPlay muted ref={videoRef}></video>
			</div>
		</div>
	);
}
