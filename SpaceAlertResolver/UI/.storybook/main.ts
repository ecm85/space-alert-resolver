import type { StorybookConfig } from '@storybook/react-vite';
import { join, dirname } from 'path';

function getAbsolutePath(value: string) {
	return dirname(require.resolve(join(value, 'package.json')));
}

const config: StorybookConfig = {
	stories: ['../src/**/*.stories.tsx'],
	addons: [getAbsolutePath('@storybook/addon-essentials')],
	framework: {
		name: getAbsolutePath('@storybook/react-vite') as any,
		options: {},
	},
	docs: {
		autodocs: 'tag',
	},
	staticDirs: ['../storybook-public'],
};

export default config;
