export interface BarcodeDataProps {
	barcodes: DetectedBarcode[];
}

export function useBarcodeData({ barcodes }: BarcodeDataProps) {
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
		const value = barcode.rawValue.split('*').slice(1, -1).join('*');

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

	return { barcodeData: barcodes.map(barcode => getBarcodeData(barcode)) };
}
