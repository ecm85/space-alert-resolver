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
            }
            var selectPhase = function (newPhaseIndex) {
                currentPhaseIndex = newPhaseIndex;
                selectSubPhase(0);
            }
            var selectTurn = function(newTurnIndex) {
                currentTurnIndex = newTurnIndex;
                selectPhase(0);
            }

            var play = function() {
                $scope.playing = $interval(function() {
                        if (!$scope.isAtLastSubPhase())
                            selectSubPhase(currentSubPhaseIndex + 1);
                        else if (!$scope.isAtLastPhase())
                            selectPhase(currentPhaseIndex + 1);
                        else if (!$scope.isAtLastTurn())
                            selectTurn(currentTurnIndex + 1);
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

            $scope.isAtFirstSubPhase = function() {
                return currentSubPhaseIndex === 0;
            }
            $scope.isAtFirstPhase = function() {
	            return currentPhaseIndex === 0;
            }
            $scope.isAtFirstTurn = function() {
                return currentTurnIndex === 0;
            }
            $scope.isAtLastSubPhase = function() {
                return currentSubPhaseIndex === $scope.getCurrentPhase().subPhases.length - 1;
            }
            $scope.isAtLastPhase = function() {
                return currentPhaseIndex === $scope.getCurrentTurn().phases.length - 1;
            }
            $scope.isAtLastTurn = function() {
                return currentTurnIndex === turnCount - 1;
            }

            $scope.getCurrentTurn = function() {
                return $scope.gameData[currentTurnIndex];
            }

            $scope.getCurrentPhase = function() {
                return $scope.getCurrentTurn().phases[currentPhaseIndex];
            }

            $scope.getCurrentSubPhase = function() {
                return $scope.getCurrentPhase().subPhases[currentSubPhaseIndex];
            }

            $scope.goBackOneSubPhase = function () {
                stop();
                if (!$scope.isAtFirstSubPhase())
                    selectSubPhase(currentSubPhaseIndex - 1);
                else if (!$scope.isAtFirstPhase()) {
                    selectPhase(currentPhaseIndex - 1);
                    selectSubPhase($scope.getCurrentPhase().subPhases.length - 1);
                }
                else if (!$scope.isAtFirstTurn()) {
                    selectTurn(currentTurnIndex - 1);
                    selectPhase($scope.getCurrentTurn().phases.length - 1);
                    selectSubPhase($scope.getCurrentPhase().subPhases.length - 1);
                }
            }
            $scope.goForwardOneSubPhase = function () {
                stop();
                if (!$scope.isAtLastSubPhase())
                    selectSubPhase(currentSubPhaseIndex + 1);
                else if (!$scope.isAtLastPhase()) {
                    selectPhase(currentPhaseIndex + 1);
                }
                else if (!$scope.isAtLastTurn()) {
                    selectTurn(currentTurnIndex + 1);
                }
            }

            $scope.goBackOnePhase = function () {
                stop();
                if (!$scope.isAtFirstPhase())
                    selectPhase(currentPhaseIndex - 1);
                else if (!$scope.isAtFirstTurn()) {
                    selectTurn(currentTurnIndex - 1);
                    selectPhase($scope.getCurrentTurn().phases.length - 1);
                }
            	//TODO: Add else to go to beginning of game (will result in changing subphases)?
            }
            $scope.goForwardOnePhase = function() {
                stop();
                if (!$scope.isAtLastPhase())
                    selectPhase(currentPhaseIndex + 1);
                else if (!$scope.isAtLastTurn()) {
                    selectTurn(currentTurnIndex + 1);
                }
            	//TODO: Add else to go to end of game (will result in changing subphases)?
            }

            $scope.goBackOneTurn = function () {
                stop();
                if (!$scope.isAtFirstTurn()) {
                    selectTurn(currentTurnIndex - 1);
                }
				//TODO: Add else to go to beginning of game (will result in changing phase and subphase)?
            }
            $scope.goForwardOneTurn = function () {
                stop();
                if (!$scope.isAtLastTurn()) {
                    selectTurn(currentTurnIndex + 1);
                }
            	//TODO: Add else to go to end of game (will result in changing phase and subphase)?
            }

            $scope.playPause = function() {
                if ($scope.playing)
                    stop();
                else
                    play();
            }
            $scope.goToStart = function() {
                stop();
                selectTurn(0);
            }
            $scope.goToEnd = function() {
                stop();
                selectTurn(turnCount - 1);
                selectPhase($scope.getCurrentTurn().phases.length - 1);
                selectSubPhase($scope.getCurrentPhase().subPhases.length - 1);
            }
            $scope.$on('$destroy',
                function() {
                    stop();
                });

            $scope.getActionCursor = function() {
                return currentTurnIndex;
            }

            selectTurn(0);
        }
    ]);
