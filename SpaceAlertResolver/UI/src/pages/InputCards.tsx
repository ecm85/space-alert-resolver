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
		setBarcodes(barcodesInOrder(newBarcodes));
	};

	const sortByX = (barcodes: DetectedBarcode[]) => {
		return [...barcodes].sort((first, second) => first.boundingBox.left - second.boundingBox.left);
	};

	const areValidBoards = (barcodes: DetectedBarcode[]) => {
		const sortedByX = sortByX(barcodes);
		const dividingLine =
			sortedByX[7].boundingBox.bottom +
			(sortedByX[12].boundingBox.top - sortedByX[7].boundingBox.bottom) / 2;
		const pairs = [
			[1, 8],
			[2, 9],
			[3, 10],
			[4, 10],
			[5, 12]
		];
		const pairsValid = pairs.every(pair => pairIsValid(sortedByX[pair[0]], sortedByX[pair[1]]));
		return (
			pairsValid &&
			sortedByX
				.slice(7)
				.every(barcode => barcode.cornerPoints.every(point => point.y < dividingLine))
		);
	};

	const pairIsValid = (first: DetectedBarcode, second: DetectedBarcode) => {
		const upperCode = first.boundingBox.bottom > second.boundingBox.bottom ? first : second;
		const lowerCode = upperCode === first ? second : first;
		const lowerCodeCenter = lowerCode.boundingBox.left + lowerCode.boundingBox.width / 2;
		const upperCodeLeft = upperCode.boundingBox.left;
		const upperCodeRight = upperCode.boundingBox.left + upperCode.boundingBox.width;
		return lowerCodeCenter > upperCodeLeft && lowerCodeCenter < upperCodeRight;
	};

	const validateBarcodes = (barcodes: DetectedBarcode[]) => {
		if (barcodes.length != 12) {
			return <div>Looking for 12 barcodes. Found: {barcodes.length}.</div>;
		}
		if (!areValidBoards(barcodes)) {
			return (
				<div>
					Barcodes are not aligned correctly. Ensure the codes for 8-12 are directly below the codes
					for 1-5
				</div>
			);
		}
		return null;
	};

	const barcodesInOrder = (barcodes: DetectedBarcode[]) => {
		const sortedByX = sortByX(barcodes);
		const dividingLine =
			sortedByX[7].boundingBox.bottom +
			(sortedByX[12].boundingBox.top - sortedByX[7].boundingBox.bottom) / 2;
		const top = [];
		const bottom = [];
		for (const barcode of barcodes) {
			if (barcode.boundingBox.top > dividingLine) {
				top.push(barcode);
			} else {
				bottom.push(barcode);
			}
		}
		return [...sortByX(top), ...sortByX(bottom)];
	};

	const getSpecialization = (index: number) => {
		switch (index) {
			case 0:
				return 'Rocketeer';
			case 1:
				return 'DataAnalyst';
			case 2:
				return 'EnergyTechnician';
			case 3:
				return 'PulseGunner';
			case 4:
				return 'Medic';
			case 5:
				return 'Teleporter';
			case 6:
				return 'Hypernavigator';
			case 7:
				return 'SpecialOps';
			case 8:
				return 'SquadLeader';
			case 9:
				return 'Mechanic';
			case 10:
		}
	};

	const getBarcodeData = (barcode: DetectedBarcode) => {
		const value = barcode.rawValue;

		if (value.startsWith('S')) {
			const index = +value.substring(1, 1);
			const specialization = getSpecialization(index);
			const levelCode = value.substring(2, 1);
			const basic = `Basic${specialization}`;
			const advanced = `Advanced${specialization}`;
			// TODO: Orientation (for level 2)
			return levelCode == 'a' ? [basic] : [basic, advanced];
		}

		if (value.startsWith('H')) {
			// TODO: Orientation
			return getHeroicCards(value);
		}

		if (value.length == 3) {
			const playerSpot = +value.substring(0, 2);
			if (Number.isNaN(playerSpot)) {
				const color = getColor(value.substring(2, 1));
				return [`${value.substring(0, 2)}${color}`];
			}
		}
		const foo = value
			.replace('L', '<')
			.replace('R', '>')
			.replace('D', '^')
			.replace('X', 'BattleBots')
			.replace('<', 'Red')
			.replace('>', 'Blue')
			.replace('^', 'Down');
		// TODO: Orientation
		return [`${foo}`];
	};

	const getHeroicCards = (code: string) => {
		switch (code) {
			case 'H1':
				return ['HeroicA', 'TeleportLowerBlue'];
			case 'H2':
				return ['HeroicA', 'TeleportLowerRed'];
			case 'H3':
				return ['HeroicB', 'TeleportUpperRed'];
			case 'H4':
				return ['HeroicB', 'TeleportUpperBlue'];
			case 'H5':
				return ['HeroicBattleBots', 'TeleportUpperWhite'];
			case 'H6':
				return ['HeroicBattleBots', 'TeleportLowerWhite'];
		}
	};

	const getColor = (value: string) => {
		switch (value) {
			case 'B':
				return 'Blue';
			case 'G':
				return 'Green';
			case 'P':
				return 'Purple';
			case 'R':
				return 'Red';
			case 'Y':
				return 'Yellow';
		}
	};

	const barcodeData = barcodes.map(barcode => getBarcodeData(barcode));

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
