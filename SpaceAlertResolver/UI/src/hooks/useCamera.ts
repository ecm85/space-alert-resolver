import { useState } from 'react';

export interface useCameraProps {
	scan(): Promise<void>;
	videoRef: React.MutableRefObject<HTMLVideoElement>;
}

export const useCamera = ({ scan, videoRef }: useCameraProps) => {
	const initialError = '';
	const [mediaStream, setMediaStream] = useState<MediaStream>(null);
	const [cameraLogs, setCameraLogs] = useState<string[]>([]);
	const [error, setError] = useState(initialError);
	const [cameraStarted, setCameraStarted] = useState(false);
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
				setCameraStarted(true);
			} else {
				setError('Could not start camera.');
			}
		};
		startPreferredCamera()
			.catch(exception => {
				setError(exception);
			})
			.finally(() => {
				setCameraLogs(newCameraLogs);
				console.log(newCameraLogs);
			});
	};

	const stopCamera = () => {
		setCameraStarted(false);
		videoRef.current.src = '';
		if (mediaStream) {
			const tracks = mediaStream.getTracks();
			for (let i = 0; i < tracks.length; i++) {
				tracks[i].stop();
			}
		}
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

	return { cameraStarted, startCamera, stopCamera, videoRef, cameraLogs, error };
};
