import { Orientation, PlayerColor, ScannedCard } from '~/models';
import { useBarcodeOrientation } from './useBarcodeOrientation';
import { DetectedBarcode } from 'barcode-detector';

export function useScannedCards() {
	const { getOrientation } = useBarcodeOrientation();
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
		}
	};

	const parseBarcode = (
		barcode: DetectedBarcode,
	): { scannedCard: ScannedCard; playerColor: PlayerColor | null } => {
		const value = barcode.rawValue.split('*').slice(1, -1).join('*');
		const orientation = getOrientation(barcode);
		const scannedCard = getScannedCard(value, orientation);
		const playerColor = tryParseEmptySpot(value)?.color ?? null;
		return { scannedCard, playerColor };
	};

	const convertStandardCard = (token: string) => {
		return token
			.replace('L', '<')
			.replace('R', '>')
			.replace('D', '^')
			.replace('X', 'BattleBots')
			.replace('<', 'Red')
			.replace('>', 'Blue')
			.replace('^', 'Down');
	};

	const getScannedCard = (value: string, orientation: Orientation): ScannedCard => {
		if (value.indexOf('*') != -1) {
			const tokens = value.split('*');
			const rawTop = tokens[0];
			const rawBottom = tokens[1];
			const isSingleAction = rawTop.length === 1;
			const movements = ['L', 'R', 'D'];
			const back = isSingleAction
				? 'SingleBack'
				: movements.includes(rawTop.charAt(0))
					? 'MoveXDoubleBack'
					: 'ActionXDoubleBack';
			return {
				type: 'split',
				top: convertStandardCard(rawTop),
				bottom: convertStandardCard(rawBottom),
				back,
				orientation,
			};
		}

		const { specialization, levelCode } = tryParseSpecialization(value);
		if (specialization != null) {
			const basic = `Basic${specialization}`;
			const advanced = `Advanced${specialization}`;
			if (levelCode == 'a') {
				return { type: 'whole', front: `Level1${basic}`, back: 'Level1SpecializationBack' };
			}
			const top = `Level2${basic}`;
			const bottom = `Level2${advanced}`;
			return { type: 'split', top, bottom, back: 'Level2SpecializationBack', orientation };
		}

		if (value.startsWith('H')) {
			return getHeroicCards(value, orientation);
		}

		const { playerSpot } = tryParseEmptySpot(value);
		if (playerSpot != null) {
			return null;
		}

		throw new Error(`Unexpected barcode: ${value}`);
	};

	const tryParseEmptySpot = (value: string) => {
		if (value.length == 3) {
			const playerSpot = +value.substring(0, 2);
			if (!Number.isNaN(playerSpot)) {
				const color = getColor(value.substring(2, 3));
				return { playerSpot, color };
			}
		}
		return {};
	};

	const tryParseSpecialization = (value: string) => {
		if (value.startsWith('S')) {
			const index = +value.substring(1, 2);
			const specialization = getSpecialization(index);
			const levelCode = value.substring(2, 3);
			return { specialization, levelCode };
		}
		return {};
	};

	const getHeroicCards = (code: string, orientation: Orientation): ScannedCard => {
		switch (code) {
			case 'H1':
				return {
					type: 'split',
					top: 'HeroicA',
					bottom: 'TeleportLowerBlue',
					back: 'HeroicBack',
					orientation,
				};
			case 'H2':
				return {
					type: 'split',
					top: 'HeroicA',
					bottom: 'TeleportLowerRed',
					back: 'HeroicBack',
					orientation,
				};
			case 'H3':
				return {
					type: 'split',
					top: 'HeroicB',
					bottom: 'TeleportUpperRed',
					back: 'HeroicBack',
					orientation,
				};
			case 'H4':
				return {
					type: 'split',
					top: 'HeroicB',
					bottom: 'TeleportUpperBlue',
					back: 'HeroicBack',
					orientation,
				};
			case 'H5':
				return {
					type: 'split',
					top: 'HeroicBattleBots',
					bottom: 'TeleportUpperWhite',
					back: 'HeroicBack',
					orientation,
				};
			case 'H6':
				return {
					type: 'split',
					top: 'HeroicBattleBots',
					bottom: 'TeleportLowerWhite',
					back: 'HeroicBack',
					orientation,
				};
			default:
				throw new Error(`Unexpected heroic card: ${code}`);
		}
	};

	const getColor = (value: string) => {
		switch (value) {
			case 'B':
				return PlayerColor.Blue;
			case 'G':
				return PlayerColor.Green;
			case 'P':
				return PlayerColor.Purple;
			case 'R':
				return PlayerColor.Red;
			case 'Y':
				return PlayerColor.Yellow;
			default:
				throw new Error(`Unexpected color: ${value}`);
		}
	};

	const parseBarcodes = (barcodes: DetectedBarcode[]) => {
		const parsedBarcodes = barcodes.map((barcode) => parseBarcode(barcode));
		const scannedCards = parsedBarcodes.map((parsed) => parsed.scannedCard);
		const playerColors = parsedBarcodes.map((parsed) => parsed.playerColor);
		const playerColor =
			playerColors.length === 0 ? null : playerColors.filter((color) => color != null)[0];
		return { scannedCards, playerColor };
	};
	return { parseBarcodes };
}
