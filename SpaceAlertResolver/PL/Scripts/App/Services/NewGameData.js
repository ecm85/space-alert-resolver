var cloneAction = function (action) {
	return {
		hotkey: action.hotkey,
		displayText: action.displayText,
		description: action.description,
		firstAction: action.firstAction,
		secondAction: action.secondAction
	};
};

angular.module("spaceAlertModule")
	.factory('newGameData',
		function() {
			var newGameData = {};
			newGameData.initialize = function() {
				newGameData.selectedTracks = {
					redTrack: null,
					whiteTrack: null,
					blueTrack: null,
					internalTrack: null
				};
				newGameData.selectedThreats = {
					redThreats: [],
					whiteThreats: [],
					blueThreats: [],
					internalThreats: []
				}
				newGameData.updateAllSelectedTracks = function () {
					newGameData.allSelectedTracks = [
						newGameData.selectedTracks.redTrack,
						newGameData.selectedTracks.whiteTrack,
						newGameData.selectedTracks.blueTrack,
						newGameData.selectedTracks.internalTrack
					];
				}
				newGameData.updateAllSelectedTracks();

				newGameData.checkDuplicateRedTrack = function (track) {
					if (newGameData.selectedTracks.redTrack === track)
						newGameData.selectedTracks.redTrack = null;
				}
				newGameData.checkDuplicateWhiteTrack = function (track) {
					if (newGameData.selectedTracks.whiteTrack === track)
						newGameData.selectedTracks.whiteTrack = null;
				}
				newGameData.checkDuplicateBlueTrack = function (track) {
					if (newGameData.selectedTracks.blueTrack === track)
						newGameData.selectedTracks.blueTrack = null;
				}
				newGameData.checkDuplicateInternalTrack = function (track) {
					if (newGameData.selectedTracks.internalTrack === track)
						newGameData.selectedTracks.internalTrack = null;
				}

				newGameData.updateAllSelectedExternalThreats = function () {
					newGameData.allSelectedExternalThreats = []
						.concat(newGameData.selectedThreats.redThreats)
						.concat(newGameData.selectedThreats.whiteThreats)
						.concat(newGameData.selectedThreats.blueThreats);
				}

				newGameData.colors = ['blue', 'green', 'red', 'yellow', 'purple'];
				newGameData.playerCounts = [1, 2, 3, 4, 5];

				newGameData.players = [
					{ title: 'Captain', color: { model: newGameData.colors[0] }, actions: _.map(_.range(12), function() { return {}; }) },
					{ title: 'Player 2', color: { model: newGameData.colors[1] }, actions: _.map(_.range(12), function () { return {}; }) },
					{ title: 'Player 3', color: { model: newGameData.colors[2] }, actions: _.map(_.range(12), function () { return {}; }) },
					{ title: 'Player 4', color: { model: newGameData.colors[3] }, actions: _.map(_.range(12), function () { return {}; }) },
					{ title: 'Player 5', color: { model: newGameData.colors[4] }, actions: _.map(_.range(12), function () { return {}; }) }
				];

				newGameData.selectPlayerCount = function (newPlayerCount) {
					newGameData.selectedPlayerCountRadio = { model: newPlayerCount };
					newGameData.players.forEach(function (player, index) {
						player.isInGame = index < newPlayerCount;
					});
				}
				newGameData.selectPlayerCount(4);

				newGameData.getGameArgs = function () {
					if (!newGameData.canCreateGame())
						return '';
					var playersInGame = _.filter(newGameData.players, { isInGame: true });
					var players = _.map(playersInGame,
						function (player, index) {
							return {
								actions: player.actions,
								index: index,
								playerColor: _.findIndex(newGameData.colors, function (color) { return color === player.color.model; })
							}
						});
					var game = {
						players: players,
						redThreats: newGameData.selectedThreats.redThreats,
						whiteThreats: newGameData.selectedThreats.whiteThreats,
						blueThreats: newGameData.selectedThreats.blueThreats,
						internalThreats: newGameData.selectedThreats.internalThreats,
						redTrack: newGameData.selectedTracks.redTrack,
						whiteTrack: newGameData.selectedTracks.whiteTrack,
						blueTrack: newGameData.selectedTracks.blueTrack,
						internalTrack: newGameData.selectedTracks.internalTrack
					};
					return game;
				}

				newGameData.canCreateGame = function () {
					return newGameData.selectedTracks.redTrack != null &&
						newGameData.selectedTracks.whiteTrack != null &&
						newGameData.selectedTracks.blueTrack != null &&
						newGameData.selectedTracks.internalTrack != null;
				}
			}

			newGameData.initialize();

			return newGameData;
		});
