"use strict";

angular.module("spaceAlertModule")
	.controller("ResolutionController",
	[
		"$scope", "gameData", '$interval', '$animate', function($scope, gameData, $interval, $animate) {
			//TODO: Remove the need for this (shouldn't need to do this check, on error it sohuld never hit this controller)
			//Check if gamedata got populated - will be undefined if there was an error
			if (!gameData)
				return;

			$animate.enabled(false);
			$scope.gameData = gameData;
			var turnCount = $scope.gameData.length;
			$scope.playing = null;
			$scope.turnsPerTwoSeconds = 5;
			let currentPhaseIndex = 0;
			let currentTurnIndex = 0;
			let currentSubPhaseIndex = 0;

			var stop = function () {
				if ($scope.playing != null) {
					$interval.cancel($scope.playing);
					$scope.playing = null;
				}
			};

			var selectSubPhase = function(newSubPhaseIndex) {
				currentSubPhaseIndex = newSubPhaseIndex;
				$scope.currentSubPhase = $scope.currentPhase.subPhases[currentSubPhaseIndex];
			}
			var selectPhase = function (newPhaseIndex) {
				currentPhaseIndex = newPhaseIndex;
				$scope.currentPhase = $scope.currentTurn.phases[currentPhaseIndex];
				selectSubPhase(0);
			}
			var selectTurn = function(newTurnIndex) {
				currentTurnIndex = newTurnIndex;
				$scope.currentTurn = $scope.gameData[currentTurnIndex];
				selectPhase(0);
			}

			var selectTurnManually = function (newTurnIndex) {
				stop();
				selectTurn(newTurnIndex);
			}
			var selectTurnAutomatically = function (newTurnIndex) {
				selectTurn(newTurnIndex);
			}

			var selectPhaseManually = function (newPhaseIndex) {
				stop();
				selectPhase(newPhaseIndex);
			}
			var selectPhaseAutomatically = function (newPhaseIndex) {
				selectPhase(newPhaseIndex);
			}

			var selectSubPhaseManually = function (newSubPhaseIndex) {
				stop();
				selectSubPhase(newSubPhaseIndex);
			}
			var selectSubPhaseAutomatically = function (newSubPhaseIndex) {
				selectSubPhase(newSubPhaseIndex);
			}
			
			var play = function() {
				$scope.playing = $interval(function() {
						if (!$scope.isAtEndOfPhase())
							selectSubPhaseAutomatically(currentSubPhaseIndex + 1);
						else if (!$scope.isAtEndOfTurn())
							selectPhaseAutomatically(currentPhaseIndex + 1);
						else if (!$scope.isAtEndOfGame())
							selectTurnAutomatically(currentTurnIndex + 1);
						else {
							stop();
						}
					},
					2000 / $scope.turnsPerTwoSeconds);
			};
			$scope.$watch('turnsPerTwoSeconds',
				function() {
					if ($scope.playing != null) {
						stop();
						play();
					}
				});

			
			$scope.currentTurnIndex = function(newTurnIndex) {
				if (arguments.length) {
					selectTurnManually(newTurnIndex);
				}
				return currentTurnIndex;
			}

			$scope.currentPhaseIndex = function (newPhaseIndex) {
				if (arguments.length) {
					selectPhaseManually(newPhaseIndex);
				}
				return currentPhaseIndex;
			}

			$scope.currentSubPhaseIndex = function (newSubPhaseIndex) {
				if (arguments.length) {
					selectSubPhaseManually(newSubPhaseIndex);
				}
				return currentSubPhaseIndex;
			}
			$scope.isAtStartOfPhase = function() {
				return currentSubPhaseIndex === 0;
			}
			$scope.isAtStartOfTurn = function() {
				return currentPhaseIndex === 0 && $scope.isAtStartOfPhase();
			}
			$scope.isAtStartOfGame = function() {
				return currentTurnIndex === 0 && $scope.isAtStartOfTurn();
			}
			$scope.isAtEndOfPhase = function() {
				return currentSubPhaseIndex === $scope.currentPhase.subPhases.length - 1;
			}
			$scope.isAtEndOfTurn = function() {
				return currentPhaseIndex === $scope.currentTurn.phases.length - 1 && $scope.isAtEndOfPhase();
			}
			$scope.isAtEndOfGame = function() {
				return currentTurnIndex === turnCount - 1 && $scope.isAtEndOfTurn();
			}
			$scope.goBackOnePhase = function() {
				if (!$scope.isAtStartOfTurn())
					selectPhaseManually(currentPhaseIndex - 1);
				else if (!$scope.isAtStartOfGame()) {
					selectTurnManually(currentTurnIndex - 1);
					selectPhaseManually($scope.currentTurn.phases.length - 1);
				}
			}
			$scope.goForwardOnePhase = function() {
				if (!$scope.isAtEndOfTurn())
					selectPhaseManually(currentPhaseIndex + 1);
				else if (!$scope.isAtEndOfGame()) {
					selectTurnManually(currentTurnIndex + 1);
				}
			}
			$scope.playPause = function() {
				if ($scope.playing)
					stop();
				else
					play();
			}
			$scope.goToStart = function() {
				if (!$scope.isAtStartOfGame()) {
					stop();
					selectTurnManually(0);
				}
			}
			$scope.goToEnd = function() {
				if (!$scope.isAtEndOfGame()) {
					stop();
					selectTurnManually(turnCount - 1);
					selectPhaseManually($scope.currentTurn.length - 1);
				}
			}
			$scope.$on('$destroy',
				function() {
					stop();
				});

			$scope.getActionCursor = function() {
				return currentTurnIndex;
			}

			selectTurnManually(0);
		}
	]);
