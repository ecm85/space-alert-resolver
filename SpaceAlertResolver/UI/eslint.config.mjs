import reactRefresh from 'eslint-plugin-react-refresh';
import typescriptEslint from '@typescript-eslint/eslint-plugin';
import eslintParser from '@typescript-eslint/parser';
import globals from 'globals';
import js from '@eslint/js';
import prettierConfig from 'eslint-config-prettier';
import reactHooks from 'eslint-plugin-react-hooks';
import storybook from 'eslint-plugin-storybook';

export default [
	js.configs.recommended,
	{
		ignores: [
			'.yarn/*',
			'dist',
			'.eslintrc.cjs',
			'.pnp.*',
			'storybook-static',
			'public',
			'.storybook',
		],
	},
	{
		files: ['**/*.ts', '**/*.tsx'],
		languageOptions: {
			ecmaVersion: 2020,
			parser: eslintParser,
			globals: {
				...globals.browser,
			},
		},
		plugins: {
			'react-refresh': reactRefresh,
			'@typescript-eslint': typescriptEslint,
			'react-hooks': reactHooks,
		},
		rules: {
			...reactHooks.configs.recommended.rules,
			...prettierConfig.rules,
			...typescriptEslint.configs.recommended.rules,
			...storybook.configs.recommended.rules,
			'react-refresh/only-export-components': ['warn', { allowConstantExport: true }],
			'spaced-comment': [
				'error',
				'always',
				{
					markers: ['/'],
				},
			],
		},
	},
];
