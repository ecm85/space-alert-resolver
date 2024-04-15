module.exports = {
	presets: [
		[
			'@babel/preset-env',
			{
				modules: 'auto',
				targets: 'last 2 versions, >0.5% in US, not dead, not ie 11',
				useBuiltIns: false,
				loose: false
			}
		],
		'@babel/preset-react',
		'@babel/preset-typescript'
	]
};
