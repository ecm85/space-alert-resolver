import { Button } from '@mui/material';
import React, { useEffect, useRef, useState } from 'react';
import { BarcodeScanningWorkflow } from '~/components/BarcodeScanningWorkflow';
import { useBarcodeData } from '~/hooks';
import { useCardScanning } from '~/hooks/useCardScanning';
import { MessageEventData } from '~/models';
import styles from './InputCards.css';

export interface InputCardsProps {
	gameCode: string;
	connection: WebSocket;
}

export function InputCards({ gameCode, connection }: InputCardsProps) {
	const [message, setMessage] = useState<string>(null);
	const [barcodes, setBarcodes] = useState<DetectedBarcode[]>([]);
	const cameraCanvasRef = useRef<HTMLCanvasElement>(null);
	const barcodeCanvasRef = useRef<HTMLCanvasElement>(null);
	const { barcodeData } = useBarcodeData({ barcodes });

	const { getBarcodesInOrder, drawDetectedBarcodes, validateBarcodes } = useCardScanning();

	useEffect(() => {
		connection.onmessage = messageEvent => {
			const messageEventData = JSON.parse(messageEvent.data) as MessageEventData;
			switch (messageEventData.event) {
				case 'ExpiredGameCode':
					setMessage('That code is expired.');
					break;
				case 'InvalidGameCode':
					setMessage('That code is invalid.');
					break;
				case 'YourMessageSent':
					setMessage('Your cards have been sent to the host.');
					break;
				default:
					console.log(`Event unhandled: ${messageEvent.data}`);
			}
		};
	}, []);

	const handleClear = () => {
		setBarcodes([]);
		const cameraContext = cameraCanvasRef.current.getContext('2d');
		cameraContext.clearRect(0, 0, cameraCanvasRef.current.width, cameraCanvasRef.current.height);
		const barcodeContext = barcodeCanvasRef.current.getContext('2d');
		barcodeContext.clearRect(0, 0, barcodeCanvasRef.current.width, barcodeCanvasRef.current.height);
	};

	const handleBarcodesScanned = (
		newBarcodes: DetectedBarcode[],
		canvas: CanvasRenderingContext2D
	) => {
		const { barcodesInOrder, dividingLine } = getBarcodesInOrder(newBarcodes);
		drawDetectedBarcodes(barcodesInOrder, dividingLine, canvas);
		const errors = validateBarcodes(barcodesInOrder, dividingLine);
		if (!errors) {
			setBarcodes(barcodesInOrder);
		}
		return errors;
	};

	const handleSendToServerClicked = () => {
		const data = {
			barcodeData
		};
		connection.send(JSON.stringify({ action: 'SendToHost', data: { code: gameCode, data } }));
	};

	return (
		<div className={styles['root']}>
			<h2>GameCode: {gameCode}</h2>
			{message && <div>{message}</div>}
			<BarcodeScanningWorkflow
				onBarcodesScan={handleBarcodesScanned}
				barcodeCanvasRef={barcodeCanvasRef}
				cameraCanvasRef={cameraCanvasRef}
				onClear={handleClear}
			/>
			<div className={styles['canvas-wrapper']}>
				<canvas className={styles['canvas']} ref={cameraCanvasRef}></canvas>
				<canvas className={styles['canvas-overlay']} ref={barcodeCanvasRef}></canvas>
			</div>
			{barcodeData.length > 0 && (
				<div>
					<Button onClick={handleSendToServerClicked} variant='contained'>
						Submit Cards
					</Button>
				</div>
			)}
		</div>
	);
}
