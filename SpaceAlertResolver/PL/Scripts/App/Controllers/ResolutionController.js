"use strict";

angular.module("spaceAlertModule")
	.controller("ResolutionController",
	[
		"$scope", "gameData", '$interval', '$animate', function($scope, gameData, $interval, $animate) {
			$animate.enabled(false);
			$scope.gameData = gameData;
			var turnCount = $scope.gameData.length;
			$scope.playing = null;
			$scope.turnsPerTwoSeconds = 5;
			let phaseIndex = 0;
			let turnIndex = 0;
			var selectPhase = function (newPhaseIndex) {
				phaseIndex = newPhaseIndex;
				$scope.currentPhase = $scope.currentTurn[phaseIndex];
			}
			var selectTurn = function(newTurnIndex) {
				turnIndex = newTurnIndex;
				$scope.currentTurn = $scope.gameData[turnIndex];
				selectPhase(0);
			}
			var stop = function() {
				if ($scope.playing != null) {
					$interval.cancel($scope.playing);
					$scope.playing = null;
				}
			};
			var play = function() {
				$scope.playing = $interval(function() {
						if (!$scope.isAtEndOfTurn())
							$scope.selectPhaseAutomatically(phaseIndex + 1);
						else if (!$scope.isAtEndOfGame())
							$scope.selectTurnAutomatically(turnIndex + 1);
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

			$scope.selectTurnManually = function(newTurnIndex) {
				stop();
				selectTurn(newTurnIndex);
			}
			$scope.selectTurnAutomatically = function (newTurnIndex) {
				selectTurn(newTurnIndex);
			}

			$scope.selectPhaseManually = function(newPhaseIndex) {
				stop();
				selectPhase(newPhaseIndex);
			}
			$scope.selectPhaseAutomatically = function (newPhaseIndex) {
				selectPhase(newPhaseIndex);
			}

			$scope.isAtStartOfTurn = function() {
				return phaseIndex === 0;
			}
			$scope.isAtStartOfGame = function() {
				return turnIndex === 0 && $scope.isAtStartOfTurn();
			}
			$scope.isAtEndOfTurn = function() {
				return phaseIndex === $scope.currentTurn.length - 1;
			}
			$scope.isAtEndOfGame = function() {
				return turnIndex === turnCount - 1 && $scope.isAtEndOfTurn();
			}
			$scope.goBackOnePhase = function() {
				if (!$scope.isAtStartOfTurn())
					$scope.selectPhaseManually(phaseIndex - 1);
				else if (!$scope.isAtStartOfGame()) {
					$scope.selectTurnManually(turnIndex - 1);
					$scope.selectPhaseManually($scope.currentTurn.length - 1);
				}
			}
			$scope.goForwardOnePhase = function() {
				if (!$scope.isAtEndOfTurn())
					$scope.selectPhaseManually(phaseIndex + 1);
				else if (!$scope.isAtEndOfGame()) {
					$scope.selectTurnManually(turnIndex + 1);
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
					$scope.selectTurnManually(0);
				}
			}
			$scope.goToEnd = function() {
				if (!$scope.isAtEndOfGame()) {
					stop();
					$scope.selectTurnManually(turnCount - 1);
					$scope.selectPhaseManually($scope.currentTurn.length - 1);
				}
			}
			$scope.$on('$destroy',
				function() {
					stop();
				});

			$scope.selectTurnManually(0);
		}
	]);
