import React, { useEffect, useState } from 'react';
import { BarcodeScanner } from '~/components/BarcodeScanner';
import { MessageEventData } from '~/models';

export interface InputCardsProps {
	gameCode: string;
	connection: WebSocket;
}

export function InputCards({ gameCode, connection }: InputCardsProps) {
	const [message, setMessage] = useState<string>(null);
	const [barcodes, setBarcodes] = useState<DetectedBarcode[]>([]);
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
		setBarcodes(newBarcodes);
	};
	return (
		<>
			<h2>GameCode: {gameCode}</h2>
			{message && <div>{message}</div>}
			{barcodes.length === 0 && <BarcodeScanner onBarcodesScan={handleBarcodesScanned} />}
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
		</>
	);
}
