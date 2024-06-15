import type { Meta, StoryObj } from '@storybook/react';
import { PlayerBoard } from './PlayerBoard';
import { Orientation, PlayerColor, ScannedCard } from '~/models';

const meta = {
	title: 'Pages/PlayerBoard',
	component: PlayerBoard,
} satisfies Meta<typeof PlayerBoard>;

export default meta;
type Story = StoryObj<typeof meta>;

const scannedCards = [
	{
		type: 'split',
		top: 'A',
		bottom: 'Down',
		back: 'SingleBack',
		orientation: Orientation.Top,
	},
	{
		type: 'split',
		top: 'B',
		bottom: 'Red',
		back: 'SingleBack',
		orientation: Orientation.Bottom,
	},
	{
		type: 'whole',
		front: 'Level1BasicEnergyTechnician',
		back: 'Level1SpecializationBack',
	},
	null,
	{
		type: 'split',
		top: 'HeroicA',
		bottom: 'TeleportLowerBlue',
		back: 'HeroicBack',
		orientation: Orientation.Top,
	},
	{
		type: 'split',
		top: 'HeroicB',
		bottom: 'TeleportUpperRed',
		back: 'HeroicBack',
		orientation: Orientation.Bottom,
	},
	{
		type: 'split',
		top: 'Level2BasicEnergyTechnician',
		bottom: 'Level2AdvancedEnergyTechnician',
		back: 'Level2SpecializationBack',
		orientation: Orientation.Top,
	},
	{
		type: 'split',
		top: 'Level2BasicMechanic',
		bottom: 'Level2AdvancedMechanic',
		back: 'Level2SpecializationBack',
		orientation: Orientation.Bottom,
	},
	{
		type: 'split',
		top: 'ARed',
		bottom: 'Down',
		back: 'ActionXDoubleBack',
		orientation: Orientation.Top,
	},
	{
		type: 'split',
		top: 'BC',
		bottom: 'Red',
		back: 'ActionXDoubleBack',
		orientation: Orientation.Bottom,
	},
	{
		type: 'split',
		top: 'RedRed',
		bottom: 'B',
		back: 'MoveXDoubleBack',
		orientation: Orientation.Top,
	},
	{
		type: 'split',
		top: 'DownBattleBots',
		bottom: 'C',
		back: 'MoveXDoubleBack',
		orientation: Orientation.Bottom,
	},
] as ScannedCard[];

export const Standard: Story = {
	args: {
		playerColor: PlayerColor.Blue,
		scannedCards,
	},
};

export const Hidden: Story = {
	args: {
		playerColor: PlayerColor.Blue,
		scannedCards,
		hideCards: true,
	},
};
