"use strict";

angular.module("spaceAlertModule").controller("ResolutionController", ["$scope", "gameData", function ($scope, gameData) {
	$scope.gameData = gameData;
	$scope.currentTurn = 0;
	$scope.selectTurn = function(turn) {
		$scope.currentTurn = turn;
	}
}]);
