import React, { useEffect, useRef, useState } from 'react';
import { BarcodeScanner } from '~/components/BarcodeScanner';
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
	const canvasRef = useRef<HTMLCanvasElement>(null);
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

	const handleBarcodesScanned = (newBarcodes: DetectedBarcode[]) => {
		const { barcodesInOrder } = getBarcodesInOrder(newBarcodes);
		setBarcodes(barcodesInOrder);
	};

	return (
		<>
			<h2>GameCode: {gameCode}</h2>
			{message && <div>{message}</div>}
			<BarcodeScanner
				onBarcodesScan={handleBarcodesScanned}
				validateBarcodes={validateBarcodes}
				drawDetectedBarcodes={drawDetectedBarcodes}
				canvasRef={canvasRef}
			/>
			<canvas className={styles['canvas']} ref={canvasRef}></canvas>
			<div>
				{barcodeData.length > 0 &&
					barcodeData.map((barcodeData, index) => (
						<div>
							Barcode {index + 1}:{' '}
							{barcodeData.map(barcodePiece => (
								<div>{barcodePiece}</div>
							))}
						</div>
					))}
			</div>
		</>
	);
}
