"use strict";

angular.module("spaceAlertModule").controller("ResolutionController", ["$scope", "gameData", function ($scope, gameData) {
	$scope.gameData = gameData;

	$scope.selectPhase = function (phase) {
		$scope.currentPhase = phase;
	}
	$scope.selectTurn = function (turn) {
		$scope.currentTurn = turn;
		$scope.selectPhase(0);
	}

	$scope.selectTurn(0);
}]);
