import { Orientation, PlayerColor } from '~/models';
import { useBarcodeOrientation } from './useBarcodeOrientation';

export interface BarcodeDataProps {
	barcodes: DetectedBarcode[];
}

export function useBarcodeData({ barcodes }: BarcodeDataProps) {
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

	const parseBarcode = (barcode: DetectedBarcode) => {
		const value = barcode.rawValue.split('*').slice(1, -1).join('*');
		const orientation = getOrientation(barcode);
		const barcodeData = getBarcodeDataToken(value, orientation);
		const playerColor = tryParseEmptySpot(value)?.color;
		return { barcodeData, playerColor };
	};

	const GetTokenForOrientation = (tokens: string[], orientation: Orientation) => {
		switch (orientation) {
			case Orientation.Top:
				return tokens[0];
			case Orientation.Bottom:
				return tokens[1];
			case Orientation.Other:
			default:
				throw new Error(`Unexpected orientation: ${orientation}`);
		}
	};

	const GetSpecializationForOrientation = (
		basic: string,
		advanced: string,
		orientation: Orientation
	) => {
		switch (orientation) {
			case Orientation.Top:
				return basic;
			case Orientation.Bottom:
				return advanced;
			case Orientation.Other:
			default:
				throw new Error(`Unexpected orientation: ${orientation}`);
		}
	};

	const GetHeroicCardForOrientation = (top: string, bottom: string, orientation: Orientation) => {
		switch (orientation) {
			case Orientation.Top:
				return top;
			case Orientation.Bottom:
				return bottom;
			case Orientation.Other:
			default:
				throw new Error(`Unexpected orientation: ${orientation}`);
		}
	};

	const getBarcodeDataToken = (value: string, orientation: Orientation) => {
		if (value.indexOf('*') != -1) {
			const tokens = value.split('*');
			const token = GetTokenForOrientation(tokens, orientation);
			const replaced = token
				.replace('L', '<')
				.replace('R', '>')
				.replace('D', '^')
				.replace('X', 'BattleBots')
				.replace('<', 'Red')
				.replace('>', 'Blue')
				.replace('^', 'Down');
			return replaced;
		}

		const { specialization, levelCode } = tryParseSpecialization(value);
		if (specialization != null) {
			const basic = `Basic${specialization}`;
			const advanced = `Advanced${specialization}`;
			return levelCode == 'a'
				? basic
				: GetSpecializationForOrientation(basic, advanced, orientation);
		}

		if (value.startsWith('H')) {
			return getHeroicCards(value, orientation);
		}

		const { playerSpot } = tryParseEmptySpot(value);
		if (playerSpot != null) {
			return `${playerSpot}`;
		}
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
			const index = +value.substring(1, 1);
			const specialization = getSpecialization(index);
			const levelCode = value.substring(2, 1);
			return { specialization, levelCode };
		}
		return {};
	};

	const getHeroicCards = (code: string, orientation: Orientation) => {
		switch (code) {
			case 'H1':
				return GetHeroicCardForOrientation('HeroicA', 'TeleportLowerBlue', orientation);
			case 'H2':
				return GetHeroicCardForOrientation('HeroicA', 'TeleportLowerRed', orientation);
			case 'H3':
				return GetHeroicCardForOrientation('HeroicB', 'TeleportUpperRed', orientation);
			case 'H4':
				return GetHeroicCardForOrientation('HeroicB', 'TeleportUpperBlue', orientation);
			case 'H5':
				return GetHeroicCardForOrientation('HeroicBattleBots', 'TeleportUpperWhite', orientation);
			case 'H6':
				return GetHeroicCardForOrientation('HeroicBattleBots', 'TeleportLowerWhite', orientation);
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
		}
	};

	const parsedBarcodes = barcodes.map(barcode => parseBarcode(barcode));
	const barcodeData = parsedBarcodes.map(parsed => parsed.barcodeData);
	const playerColors = parsedBarcodes.map(parsed => parsed.playerColor);
	const playerColor = playerColors.filter(color => color != null)[0];
	return { barcodeData, playerColor };
}
