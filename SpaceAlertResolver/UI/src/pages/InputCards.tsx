import React, { useEffect, useRef, useState } from 'react';
import { BarcodeScanner } from '~/components/BarcodeScanner';
import { useBarcodeData } from '~/hooks';
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

	const handleBarcodesScanned = (newBarcodes: DetectedBarcode[], newImageData: ImageData) => {
		const sortedBarcodes = barcodesInOrder(newBarcodes);
		setBarcodes(sortedBarcodes);
		const canvas = canvasRef.current.getContext('2d');
		canvasRef.current.height = newImageData.height;
		canvasRef.current.width = newImageData.width;
		canvas.putImageData(newImageData, 0, 0);
		let index = 1;
		for (const barcode of sortedBarcodes) {
			canvas.fillStyle = 'blue';
			canvas.fillRect(
				barcode.boundingBox.left,
				barcode.boundingBox.top,
				barcode.boundingBox.width,
				barcode.boundingBox.height
			);
			canvas.fillStyle = 'red';
			canvas.fillText(index.toString(), barcode.boundingBox.left, barcode.boundingBox.top);
			index++;
		}
	};

	const sortByX = (barcodes: DetectedBarcode[]) => {
		return [...barcodes].sort((first, second) => first.boundingBox.left - second.boundingBox.left);
	};

	// const pairIsValid = (first: DetectedBarcode, second: DetectedBarcode, dividingLine: number) => {
	// 	const upperCode = first.boundingBox.bottom > second.boundingBox.bottom ? second : first;
	// 	const lowerCode = upperCode === first ? second : first;
	// 	const lowerCodeCenter = lowerCode.boundingBox.left + lowerCode.boundingBox.width / 2;
	// 	const upperCodeLeft = upperCode.boundingBox.left;
	// 	const upperCodeRight = upperCode.boundingBox.left + upperCode.boundingBox.width;
	// 	const xAlignmentIsCorrect = lowerCodeCenter > upperCodeLeft && lowerCodeCenter < upperCodeRight;
	// 	const yAlignmentIsCorrect =
	// 		upperCode.boundingBox.bottom < dividingLine && lowerCode.boundingBox.top > dividingLine;
	// 	return xAlignmentIsCorrect && yAlignmentIsCorrect;
	// };

	const getDividingLine = (first: DetectedBarcode, second: DetectedBarcode) => {
		const upperCode = first.boundingBox.bottom > second.boundingBox.bottom ? second : first;
		const lowerCode = upperCode === first ? second : first;
		return (
			upperCode.boundingBox.bottom + (lowerCode.boundingBox.top - upperCode.boundingBox.bottom) / 2
		);
	};

	const validateBarcodes = (barcodes: DetectedBarcode[]) => {
		if (barcodes.length != 12) {
			return <div>Looking for 12 barcodes. Found: {barcodes.length}.</div>;
		}
		// console.log(barcodes);
		// const sortedByX = sortByX(barcodes);
		// const pairs = [
		// 	[0, 7],
		// 	[1, 8],
		// 	[2, 9],
		// 	[3, 10],
		// 	[4, 11]
		// ];
		// const trailingCards = [5, 6];
		// const dividingLine = getDividingLine(sortedByX[0], sortedByX[1]);
		// console.log(dividingLine);
		// const invalidPairs = pairs.filter(
		// 	pair => !pairIsValid(sortedByX[pair[0]], sortedByX[pair[1]], dividingLine)
		// );
		// if (invalidPairs.length > 0) {
		// 	return (
		// 		<div>
		// 			Barcodes are not aligned correctly. Ensure the codes for 8-12 are directly below the codes
		// 			for 1-5. Codes out of alignment:
		// 			{invalidPairs.map(invalidPair => (
		// 				<div>
		// 					{invalidPair[0] + 1} is not aligned with {invalidPair[1] + 1}
		// 				</div>
		// 			))}
		// 		</div>
		// 	);
		// }
		// const invalidTrailingCards = trailingCards.filter(
		// 	cardIndex => !sortedByX[cardIndex].cornerPoints.every(point => point.y < dividingLine)
		// );
		// if (invalidTrailingCards.length > 0) {
		// 	return (
		// 		<div>
		// 			Barcodes are not aligned correctly. Ensure there's a clear line between the top and bottom
		// 			boards.
		// 		</div>
		// 	);
		// }
		// return null;
	};

	const barcodesInOrder = (barcodes: DetectedBarcode[]) => {
		const sortedByX = sortByX(barcodes);
		const dividingLine = getDividingLine(sortedByX[0], sortedByX[1]);
		const top = [];
		const bottom = [];
		for (const barcode of barcodes) {
			if (barcode.boundingBox.bottom < dividingLine) {
				top.push(barcode);
			} else {
				bottom.push(barcode);
			}
		}
		return [...sortByX(top), ...sortByX(bottom)];
	};

	return (
		<>
			<h2>GameCode: {gameCode}</h2>
			{message && <div>{message}</div>}
			{barcodes.length === 0 && (
				<BarcodeScanner
					onBarcodesScan={handleBarcodesScanned}
					validateBarcodes={validateBarcodes}
				/>
			)}
			{barcodes && <canvas className={styles['canvas']} ref={canvasRef}></canvas>}
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
