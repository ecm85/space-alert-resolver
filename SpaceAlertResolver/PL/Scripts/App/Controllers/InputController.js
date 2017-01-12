"use strict";

var cloneAction = function(action) {
	return {
		hotkey: action.hotkey,
		displayText: action.displayText,
		description: action.description,
		action: action.action
	};
};

angular.module("spaceAlertModule")
	.controller("InputController",
	[
		"$scope", '$uibModal', 'inputData', function($scope, $uibModal, inputData) {
			$scope.allTracks = inputData.tracks;
			$scope.selectedTracks = {
				redTrack: null,
				whiteTrack: null,
				blueTrack: null,
				internalTrack: null
			};
			var updateAllSelectedTracks = function() {
				$scope.allSelectedTracks = [
					$scope.selectedTracks.redTrack,
					$scope.selectedTracks.whiteTrack,
					$scope.selectedTracks.blueTrack,
					$scope.selectedTracks.internalTrack
				];
			}
			updateAllSelectedTracks();
			$scope.$watch('selectedTracks.redTrack',
				function() {
					updateAllSelectedTracks();
				});
			$scope.$watch('selectedTracks.whiteTrack',
				function() {
					updateAllSelectedTracks();
				});
			$scope.$watch('selectedTracks.blueTrack',
				function() {
					updateAllSelectedTracks();
				});
			$scope.$watch('selectedTracks.internalTrack',
				function() {
					updateAllSelectedTracks();
				});

			var checkDuplicateRedTrack = function(track) {
				if ($scope.selectedTracks.redTrack === track)
					$scope.selectedTracks.redTrack = null;
			}
			var checkDuplicateWhiteTrack = function(track) {
				if ($scope.selectedTracks.whiteTrack === track)
					$scope.selectedTracks.whiteTrack = null;
			}
			var checkDuplicateBlueTrack = function(track) {
				if ($scope.selectedTracks.blueTrack === track)
					$scope.selectedTracks.blueTrack = null;
			}
			var checkDuplicateInternalTrack = function(track) {
				if ($scope.selectedTracks.internalTrack === track)
					$scope.selectedTracks.internalTrack = null;
			}
			$scope.$watch('selectedTracks.redTrack',
				function(newValue) {
					if (!newValue)
						return;
					checkDuplicateWhiteTrack(newValue);
					checkDuplicateBlueTrack(newValue);
					checkDuplicateInternalTrack(newValue);
				});
			$scope.$watch('selectedTracks.whiteTrack',
				function(newValue) {
					if (!newValue)
						return;
					checkDuplicateRedTrack(newValue);
					checkDuplicateBlueTrack(newValue);
					checkDuplicateInternalTrack(newValue);
				});
			$scope.$watch('selectedTracks.blueTrack',
				function(newValue) {
					if (!newValue)
						return;
					checkDuplicateRedTrack(newValue);
					checkDuplicateWhiteTrack(newValue);
					checkDuplicateInternalTrack(newValue);
				});
			$scope.$watch('selectedTracks.internalTrack',
				function(newValue) {
					if (!newValue)
						return;
					checkDuplicateRedTrack(newValue);
					checkDuplicateWhiteTrack(newValue);
					checkDuplicateBlueTrack(newValue);
				});

			$scope.allInternalThreats = inputData.allInternalThreats;
			$scope.allExternalThreats = inputData.allExternalThreats;
			$scope.selectedThreats = {
				redThreats: [],
				whiteThreats: [],
				blueThreats: [],
				internalThreats: []
			}
			var updateAllSelectedExternalThreats = function() {
				$scope.allSelectedExternalThreats = $scope.selectedThreats.redThreats.concat($scope.selectedThreats.whiteThreats)
					.concat($scope.selectedThreats.blueThreats);
			}
			$scope.$watchCollection('selectedThreats.redThreats',
				function() {
					updateAllSelectedExternalThreats();
				});
			$scope.$watchCollection('selectedThreats.whiteThreats',
				function() {
					updateAllSelectedExternalThreats();
				});
			$scope.$watchCollection('selectedThreats.blueThreats',
				function() {
					updateAllSelectedExternalThreats();
				});

			//TODO: Add specializations
			//TODO: Add double actions

			$scope.allActions = inputData.actions;

			$scope.colors = ['blue', 'green', 'red', 'yellow', 'purple'];
			$scope.playerCounts = [1, 2, 3, 4, 5];
			var createEmptyAction = function() { return cloneAction($scope.allActions[0]); };
			$scope.players = [
				{ title: 'Captain', color: { model: $scope.colors[0] }, actions: _.map(_.range(12), createEmptyAction) },
				{ title: 'Player 2', color: { model: $scope.colors[1] }, actions: _.map(_.range(12), createEmptyAction) },
				{ title: 'Player 3', color: { model: $scope.colors[2] }, actions: _.map(_.range(12), createEmptyAction) },
				{ title: 'Player 4', color: { model: $scope.colors[3] }, actions: _.map(_.range(12), createEmptyAction) },
				{ title: 'Player 5', color: { model: $scope.colors[4] }, actions: _.map(_.range(12), createEmptyAction) }
			];
			$scope.selectPlayerCount = function(newPlayerCount) {
				$scope.selectedPlayerCountRadio = { model: newPlayerCount };
				$scope.players.forEach(function(player, index) {
					player.isInGame = index < newPlayerCount;
				});
			}
			$scope.selectPlayerCount(4);
			$scope.$watch('selectedPlayerCountRadio.model',
				function(newPlayerCount) {
					$scope.selectPlayerCount(newPlayerCount);
				});
			$scope.players.forEach(function(player, index) {
				$scope.$watch(
					function(scope) {
						return scope.players[index].color.model;
					},
					function(newValue, oldValue) {
						$scope.players.forEach(function(player, exisitingPlayerIndex) {
							if (index !== exisitingPlayerIndex && player.color.model === newValue)
								player.color.model = oldValue;
						});
					});
			});

			$scope.animationsEnabled = true;

			$scope.openActionsDialog = function(player, size) {
				$uibModal.open({
					animation: true,
					templateUrl: 'templates/actionsModal',
					controller: 'ActionsModalInstanceCtrl',
					size: size,
					resolve: {
						player: function() {
							return player;
						},
						allActions: function() {
							return $scope.allActions;
						}
					}
				});
			};

			$scope.canCreateGame = function() {
				return $scope.selectedTracks.redTrack != null &&
					$scope.selectedTracks.whiteTrack != null &&
					$scope.selectedTracks.blueTrack != null &&
					$scope.selectedTracks.internalTrack != null;
			}

			$scope.getGameArgs = function() {
				if (!$scope.canCreateGame())
					return '';
				var playersInGame = _.filter($scope.players, { isInGame: true });
				var players = _.map(playersInGame,
					function(player, index) {
						return {
							actions: player.actions,
							index: index,
							playerColor: _.findIndex($scope.colors, function(color) { return color === player.color.model; })
						}
					});
				var game = {
					players: players,
					redThreats: $scope.selectedThreats.redThreats,
					whiteThreats: $scope.selectedThreats.whiteThreats,
					blueThreats: $scope.selectedThreats.blueThreats,
					internalThreats: $scope.selectedThreats.internalThreats,
					redTrack: $scope.selectedTracks.redTrack,
					whiteTrack: $scope.selectedTracks.whiteTrack,
					blueTrack: $scope.selectedTracks.blueTrack,
					internalTrack: $scope.selectedTracks.internalTrack
				};
				return JSON.stringify(game);
			}
		}
	]);
