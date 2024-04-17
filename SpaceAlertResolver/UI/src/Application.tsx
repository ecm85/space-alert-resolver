import React, { useState, useRef, useEffect } from 'react';
import { BarcodeDetector } from 'barcode-detector';
import cx from 'classnames';
import styles from './Application.css';

enum IWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Done,
	Error
}

export default function Application() {
	const canvasRef = useRef<HTMLCanvasElement>();
	const videoRef = useRef<HTMLVideoElement>();
	const [mediaStream, setMediaStream] = useState<MediaStream>(null);
	const [workflowState, setWorkflowState] = useState(IWorkflowState.Initial);
	const barcodeDetector = new BarcodeDetector();
	const [cameraLogs, setCameraLogs] = useState<string[]>([]);
	const [error, setError] = useState('');
	const [barcodes, setBarcodes] = useState<DetectedBarcode[]>([]);
	const [scanTimeoutId, setScanTimeoutId] = useState<number>(null);
	const [desiredBarcodeCount, setDesiredBarcodeCount] = useState<number>(1);
	const [detectedBarcodeCount, setDetectedBarcodeCount] = useState<number>();

	const setErrorState = (error: string) => {
		setError(error);
		setWorkflowState(IWorkflowState.Error);
	};

	const tryGetBarcodes = () => {
		if (videoRef.current.readyState !== videoRef.current.HAVE_ENOUGH_DATA) {
			return null;
		}
		const canvas = canvasRef.current.getContext('2d');
		const { videoWidth, videoHeight } = videoRef.current;
		canvasRef.current.height = videoHeight;
		canvasRef.current.width = videoWidth;
		canvas.drawImage(videoRef.current, 0, 0, videoWidth, videoHeight);
		const imageData = canvas.getImageData(0, 0, videoWidth, videoHeight);
		return barcodeDetector.detect(imageData);
	};

	const scan = async () => {
		try {
			const barcodes = await tryGetBarcodes();
			const validBarcodes = barcodes.filter(barcode => !!barcode.rawValue);
			setDetectedBarcodeCount(validBarcodes.length);
			if (barcodes?.length == desiredBarcodeCount) {
				handleScan(barcodes);
			} else {
				setScanTimeoutId(window.setTimeout(scan, 250));
			}
		} catch (error) {
			console.info(`unable to detect qr code: - ${error.message}`);
		}
	};

	const handleScan = (barcodes: DetectedBarcode[]) => {
		setBarcodes(barcodes);
		setWorkflowState(IWorkflowState.Done);
	};

	const startSpecificCameraFromStream = async (stream: MediaStream, newCameraLogs: string[]) => {
		try {
			videoRef.current.srcObject = stream;
			setMediaStream(stream);
			await scan();
			return true;
		} catch (error) {
			newCameraLogs.push(`unable to start camera: ${stream.id} - ${error.message}`);
			return false;
		}
	};

	const startSpecificCameraByEnsuringAccess = async (newCameraLogs: string[]) => {
		try {
			const initialCamera = await navigator.mediaDevices.getUserMedia({
				audio: true,
				video: { facingMode: { ideal: 'environment' } }
			});
			return await startSpecificCameraFromStream(initialCamera, newCameraLogs);
		} catch (error) {
			newCameraLogs.push(`unable to get camera: ${error.message}`);
			return false;
		}
	};

	const getCameraInfos = async () => {
		const devices = await navigator.mediaDevices.enumerateDevices();
		return devices.filter(device => device.kind === 'videoinput');
	};

	const cameraIsBack = (cameraInfo: MediaDeviceInfo) => {
		return (cameraInfo.label || '').toLowerCase().includes('back');
	};

	const orderCameraInfos = (camerasInfos: MediaDeviceInfo[]) => {
		return [...camerasInfos].sort((cameraInfo1, cameraInfo2) => {
			const camera1IsBack = cameraIsBack(cameraInfo1);
			const camera2IsBack = cameraIsBack(cameraInfo2);

			if (camera1IsBack && !camera2IsBack) {
				return -1;
			}
			if (camera2IsBack && !camera1IsBack) {
				return 1;
			}

			return 0;
		});
	};

	const startSpecificCameraFromInfo = async (
		cameraInfo: MediaDeviceInfo,
		newCameraLogs: string[]
	) => {
		try {
			const camera = await navigator.mediaDevices.getUserMedia({
				audio: true,
				video: { deviceId: { exact: cameraInfo.deviceId } }
			});
			return await startSpecificCameraFromStream(camera, newCameraLogs);
		} catch (error) {
			newCameraLogs.push(`unable to get camera: ${cameraInfo.label} - ${error.message}`);
			return false;
		}
	};

	const startPreferredCameraAsync = async (newCameraLogs: string[]) => {
		try {
			if (await startSpecificCameraByEnsuringAccess(newCameraLogs)) {
				return true;
			}
			const cameraInfos = await getCameraInfos();
			newCameraLogs.push(`got ${cameraInfos.length} camera infos`);
			let cameraIndex = 0;
			for (const cameraInfo of cameraInfos) {
				newCameraLogs.push(`camera info ${cameraIndex}: ${cameraInfo.label}`);
				cameraIndex++;
			}
			const orderedCameraInfos = orderCameraInfos(cameraInfos);
			for (const cameraInfo of orderedCameraInfos) {
				newCameraLogs.push(`trying to start ${cameraInfo.label}`);
				if (await startSpecificCameraFromInfo(cameraInfo, newCameraLogs)) {
					return true;
				}
			}
			newCameraLogs.push(`couldn't start any of ${orderCameraInfos.length} cameras`);
			return false;
		} catch (error) {
			newCameraLogs.push(`unable to access camera: ${error.message}`);
			return false;
		}
	};

	const startCamera = () => {
		const newCameraLogs: string[] = [];
		const startPreferredCamera = async () => {
			const startedCamera = await startPreferredCameraAsync(newCameraLogs);
			if (startedCamera) {
				setWorkflowState(IWorkflowState.CameraStarted);
			} else {
				setErrorState('Could not start camera.');
			}
		};
		startPreferredCamera().catch(exception => setErrorState(exception.toString()));
		setCameraLogs(newCameraLogs);
	};

	useEffect(() => {
		switch (workflowState) {
			case IWorkflowState.CameraStarting:
				startCamera();
				break;
			case IWorkflowState.Done:
				stopCamera();
		}
	}, [workflowState]);

	const stopCamera = () => {
		videoRef.current.src = '';
		if (mediaStream) {
			const tracks = mediaStream.getTracks();
			for (let i = 0; i < tracks.length; i++) {
				tracks[i].stop();
			}
		}
		if (scanTimeoutId) {
			window.clearTimeout(scanTimeoutId);
		}
	};

	const handleStartCameraClicked = () => {
		setWorkflowState(IWorkflowState.CameraStarting);
	};

	const captureVideoClassName = cx(styles.video, {
		[styles.hiddenVideo]: workflowState !== IWorkflowState.CameraStarted
	});

	const handleDesiredBarcodeCountChanged = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newDesiredBarcodeCount = +e.target.value;
		if (newDesiredBarcodeCount > 0) {
			setDesiredBarcodeCount(newDesiredBarcodeCount);
		}
	};

	return (
		<div>
			{(workflowState === IWorkflowState.Initial || workflowState === IWorkflowState.Done) && (
				<button onClick={handleStartCameraClicked}>Start Camera</button>
			)}
			<div>
				<label># of barcodes to scan</label>
				<input
					type='number'
					onChange={handleDesiredBarcodeCountChanged}
					value={desiredBarcodeCount}></input>
			</div>
			<canvas hidden ref={canvasRef}></canvas>
			<div className={styles.videoWrapper}>
				<video playsInline className={captureVideoClassName} autoPlay muted ref={videoRef}></video>
			</div>
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
		</div>
	);
}
