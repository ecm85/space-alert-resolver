import React, { useState, useRef, useEffect } from 'react';
import { BarcodeDetector } from 'barcode-detector';

enum IWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Error
}

export default function Application() {
	const canvasRef = useRef<HTMLCanvasElement>();
	const videoRef = useRef<HTMLVideoElement>();
	const [mediaStream, setMediaStream] = useState<MediaStream>(null);
	const [workflowState, setWorkflowState] = useState(IWorkflowState.Initial);
	console.log(BarcodeDetector.getSupportedFormats());
	const barcodeDetector = new BarcodeDetector();
	const [cameraLogs, setCameraLogs] = useState<string[]>([]);
	const [error, setError] = useState('');
	const [barcodes, setBarcodes] = useState<DetectedBarcode[]>([]);

	const setErrorState = (error: string) => {
		setError(error);
		setWorkflowState(IWorkflowState.Error);
	};

	const tryGetData = () => {
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
			const data = await tryGetData();
			if (data?.length) {
				handleScan(data);
			} else {
				this.scanTimeoutId = window.setTimeout(this.scan, 250);
			}
		} catch (error) {
			console.info(`unable to detect qr code: - ${error.message}`);
		}
	};

	const handleScan = (data: DetectedBarcode[]) => {
		setBarcodes(data);
		stopCamera();
	};

	const startSpecificCameraFromStream = async (stream: MediaStream) => {
		try {
			videoRef.current.srcObject = stream;
			setMediaStream(stream);
			await scan();
			return true;
		} catch (error) {
			setCameraLogs([...cameraLogs, `unable to start camera: ${stream.id} - ${error.message}`]);
			return false;
		}
	};

	const startSpecificCameraByEnsuringAccess = async () => {
		try {
			const initialCamera = await navigator.mediaDevices.getUserMedia({
				audio: true,
				video: { facingMode: { ideal: 'user' } }
			});
			return await startSpecificCameraFromStream(initialCamera);
		} catch (error) {
			setCameraLogs([...cameraLogs, `unable to get camera: ${error.message}`]);
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
				return 1;
			}
			if (camera2IsBack && !camera1IsBack) {
				return -1;
			}

			return 0;
		});
	};

	const startSpecificCameraFromInfo = async (cameraInfo: MediaDeviceInfo) => {
		try {
			const camera = await navigator.mediaDevices.getUserMedia({
				audio: true,
				video: { deviceId: { exact: cameraInfo.deviceId } }
			});
			return await startSpecificCameraFromStream(camera);
		} catch (error) {
			setCameraLogs([
				...cameraLogs,
				`unable to get camera: ${cameraInfo.label} - ${error.message}`
			]);
			return false;
		}
	};

	const startPreferredCameraAsync = async () => {
		try {
			if (await startSpecificCameraByEnsuringAccess()) {
				return true;
			}
			const cameraInfos = await getCameraInfos();
			const orderedCameraInfos = orderCameraInfos(cameraInfos);
			for (const cameraInfo of orderedCameraInfos) {
				if (await startSpecificCameraFromInfo(cameraInfo)) {
					return true;
				}
			}
			return false;
		} catch (error) {
			setCameraLogs([...cameraLogs, `unable to access camera: ${error.message}`]);
			return false;
		}
	};

	const startCamera = () => {
		const startPreferredCamera = async () => {
			const startedCamera = await startPreferredCameraAsync();
			if (startedCamera) {
				setWorkflowState(IWorkflowState.CameraStarted);
			} else {
				setErrorState(
					'Could not start camera.' +
						(cameraLogs.length ? `Errors: ${cameraLogs.join(' ----- ')}` : '')
				);
			}
		};
		startPreferredCamera().catch(exception => setErrorState(exception.toString()));
	};

	useEffect(() => {
		switch (workflowState) {
			case IWorkflowState.CameraStarting:
				startCamera();
				break;
		}
	}, [workflowState]);

	const stopCamera = () => {
		const tracks = mediaStream.getTracks();
		for (let i = 0; i < tracks.length; i++) {
			tracks[i].stop();
		}
	};

	const handleStartCameraClicked = () => {
		setWorkflowState(IWorkflowState.CameraStarting);
	};

	return (
		<div>
			{workflowState === IWorkflowState.Initial && (
				<button onClick={handleStartCameraClicked}>Start Camera</button>
			)}
			<canvas hidden ref={canvasRef}></canvas>
			{workflowState === IWorkflowState.Error && (
				<>
					<div>Uh Oh! Something went wrong.</div>
					<div>{error}</div>
				</>
			)}
			<div>
				{barcodes.length &&
					barcodes.map((barcode, index) => <div>Barcode {index + 1}: barcode.rawValue</div>)}
			</div>
		</div>
	);
}
