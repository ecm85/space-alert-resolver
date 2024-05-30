import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tsconfigPaths from 'vite-tsconfig-paths';

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [react(), tsconfigPaths()],
	server: {
		host: 'localhost',
		port: 9229,
	},
	build: {
		rollupOptions: {
			output: {
				manualChunks: {
					react: ['react', 'react-dom', 'react-router-dom'],
				},
			},
		},
	},
});
