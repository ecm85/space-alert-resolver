import type { Meta, StoryObj } from '@storybook/react';
import { ColorPicker } from './ColorPicker';
import { PlayerColor } from '~/models';
import { fn } from '@storybook/test';
const meta = {
	title: 'Pages/ColorPicker',
	component: ColorPicker,
} satisfies Meta<typeof ColorPicker>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Standard: Story = {
	args: {
		value: PlayerColor.Green,
		onPickColor: fn(),
	},
};
