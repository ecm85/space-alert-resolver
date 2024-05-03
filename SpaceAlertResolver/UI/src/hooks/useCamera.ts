import { useRef } from 'react';

export interface useCameraProps {
	processCamera(): Promise<void>;
	videoRef: React.MutableRefObject<HTMLVideoElement>;
	onError(error: string): void;
}

export const useCamera = ({ processCamera, videoRef, onError }: useCameraProps) => {
	const mediaStreamRef = useRef<MediaStream>(null);
	const cameraStartedRef = useRef(false);
	const startPreferredCameraAsync = async () => {
		try {
			if (await startSpecificCameraByEnsuringAccess()) {
				return true;
			}
			const cameraInfos = await getCameraInfos();
			console.log(`got ${cameraInfos.length} camera infos`);
			let cameraIndex = 0;
			for (const cameraInfo of cameraInfos) {
				console.log(`camera info ${cameraIndex}: ${cameraInfo.label}`);
				cameraIndex++;
			}
			const orderedCameraInfos = orderCameraInfos(cameraInfos);
			for (const cameraInfo of orderedCameraInfos) {
				console.log(`trying to start ${cameraInfo.label}`);
				if (await startSpecificCameraFromInfo(cameraInfo)) {
					return true;
				}
			}
			console.log(`couldn't start any of ${orderCameraInfos.length} cameras`);
			return false;
		} catch (error) {
			console.log(`unable to access camera: ${error.message}`);
			return false;
		}
	};

	const startCamera = () => {
		const startPreferredCamera = async () => {
			const startedCamera = await startPreferredCameraAsync();
			if (startedCamera) {
				cameraStartedRef.current = true;
			} else {
				onError('Could not start camera.');
			}
		};
		startPreferredCamera().catch(exception => {
			onError(exception);
		});
	};

	const stopCamera = () => {
		cameraStartedRef.current = false;
		if (videoRef.current) {
			videoRef.current.src = '';
		}
		if (mediaStreamRef.current) {
			const tracks = mediaStreamRef.current.getTracks();
			for (let i = 0; i < tracks.length; i++) {
				tracks[i].stop();
			}
		}
	};

	const startSpecificCameraFromInfo = async (cameraInfo: MediaDeviceInfo) => {
		try {
			const camera = await navigator.mediaDevices.getUserMedia({
				audio: true,
				video: { deviceId: { exact: cameraInfo.deviceId } }
			});
			return await startSpecificCameraFromStream(camera);
		} catch (error) {
			console.log(`unable to get camera: ${cameraInfo.label} - ${error.message}`);
			return false;
		}
	};

	const startSpecificCameraFromStream = async (stream: MediaStream) => {
		try {
			videoRef.current.srcObject = stream;
			mediaStreamRef.current = stream;
			await processCamera();
			return true;
		} catch (error) {
			console.log(`unable to start camera: ${stream.id} - ${error.message}`);
			return false;
		}
	};

	const startSpecificCameraByEnsuringAccess = async () => {
		try {
			const initialCamera = await navigator.mediaDevices.getUserMedia({
				audio: true,
				video: { facingMode: { ideal: 'environment' } }
			});
			return await startSpecificCameraFromStream(initialCamera);
		} catch (error) {
			console.log(`unable to get camera: ${error.message}`);
			return false;
		}
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

	const getCameraInfos = async () => {
		const devices = await navigator.mediaDevices.enumerateDevices();
		return devices.filter(device => device.kind === 'videoinput');
	};

	const cameraIsBack = (cameraInfo: MediaDeviceInfo) => {
		return (cameraInfo.label || '').toLowerCase().includes('back');
	};

	return { cameraStartedRef, startCamera, stopCamera, videoRef };
};
