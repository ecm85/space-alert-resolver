import type { Meta, StoryFn } from '@storybook/react';
import { ColorPicker, ColorPickerProps } from './ColorPicker';
import { PlayerColor } from '~/models';
import { useState } from 'react';
const meta = {
	title: 'Pages/ColorPicker',
	component: ColorPicker,
} satisfies Meta<typeof ColorPicker>;

export default meta;

export const Standard: StoryFn<ColorPickerProps> = () => {
	const [color, setColor] = useState<PlayerColor>(PlayerColor.Green);
	return <ColorPicker onPickColor={setColor} value={color}></ColorPicker>;
};
