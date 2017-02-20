"use strict";

angular.module("spaceAlertModule")
	.controller("InputController",
	[
		"$scope", '$uibModal', 'inputData', 'newGameData', function($scope, $uibModal, inputData, newGameData) {
			$scope.allTracks = inputData.tracks;
			$scope.newGameData = newGameData;
			$scope.playerSpecializations = inputData.playerSpecializations;

			$scope.$watch('newGameData.selectedTracks.redTrack',
				function(newValue) {
					newGameData.updateAllSelectedTracks();
					if (!newValue)
						return;
					newGameData.checkDuplicateWhiteTrack(newValue);
					newGameData.checkDuplicateBlueTrack(newValue);
					newGameData.checkDuplicateInternalTrack(newValue);
				});
			$scope.$watch('newGameData.selectedTracks.whiteTrack',
				function(newValue) {
					newGameData.updateAllSelectedTracks();
					if (!newValue)
						return;
					newGameData.checkDuplicateRedTrack(newValue);
					newGameData.checkDuplicateBlueTrack(newValue);
					newGameData.checkDuplicateInternalTrack(newValue);
				});
			$scope.$watch('newGameData.selectedTracks.blueTrack',
				function(newValue) {
					newGameData.updateAllSelectedTracks();
					if (!newValue)
						return;
					newGameData.checkDuplicateRedTrack(newValue);
					newGameData.checkDuplicateWhiteTrack(newValue);
					newGameData.checkDuplicateInternalTrack(newValue);
				});
			$scope.$watch('newGameData.selectedTracks.internalTrack',
				function(newValue) {
					newGameData.updateAllSelectedTracks();
					if (!newValue)
						return;
					newGameData.checkDuplicateRedTrack(newValue);
					newGameData.checkDuplicateWhiteTrack(newValue);
					newGameData.checkDuplicateBlueTrack(newValue);
				});

			$scope.allInternalThreats = inputData.allInternalThreats;
			$scope.allExternalThreats = inputData.allExternalThreats;

			$scope.$watchCollection('newGameData.selectedThreats.redThreats',
				function() {
					newGameData.updateAllSelectedExternalThreats();
				});
			$scope.$watchCollection('newGameData.selectedThreats.whiteThreats',
				function() {
					newGameData.updateAllSelectedExternalThreats();
				});
			$scope.$watchCollection('newGameData.selectedThreats.blueThreats',
				function() {
					newGameData.updateAllSelectedExternalThreats();
				});

			//TODO: Add specializations
			//TODO: Add double actions

			$scope.allSingleActions = inputData.singleActions;
			$scope.allDoubleActions = inputData.doubleActions;
			$scope.specializationActions = inputData.specializationActions;

			$scope.$watch('newGameData.selectedPlayerCountRadio.model',
				function(newPlayerCount) {
					newGameData.selectPlayerCount(newPlayerCount);
				});
			newGameData.players.forEach(function(player, index) {
				$scope.$watch(
					function() {
						return newGameData.players[index].color.model;
					},
					function(newValue, oldValue) {
						newGameData.players.forEach(function (player, exisitingPlayerIndex) {
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
						allSingleActions: function () {
							return $scope.allSingleActions;
						},
						allDoubleActions: function () {
							return $scope.allDoubleActions;
						},
						useDoubleActions: function() {
							return newGameData.useDoubleActions;
						},
						playerSpecializationActions: function() {
							return $scope.specializationActions.filter(function(specializationAction) {
								return specializationAction.playerSpecialization === player.playerSpecialization;
							});
						},
						useSpecializations: function () {
							return newGameData.useSpecializations && player.playerSpecialization != null;
						}
					}
				});
			};
		}
	]);
