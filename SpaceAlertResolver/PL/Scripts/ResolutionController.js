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

	$scope.getThreatCornerX = function(color, index) {
		return $('#' + color + 'Threat' + index).offset().left;
	}

	$scope.getThreatCornerY = function (color, index) {
		var threatElement = $('#' + color + 'Threat' + index);
		return threatElement.offset().top + (threatElement.height() / 2);
	}

	$scope.getTrackSpaceCornerX = function(color, threat) {
		var spaceElement = $('#' + color + 'Space' + threat.position);
		return spaceElement.offset().left + spaceElement.width();
	}

	$scope.getTrackSpaceCornerY = function (color, threat) {
		return $('#' + color + 'Space' + threat.position).offset().top;
	}

	//TODO: Check what happens when threats scroll
	//TODO: Add to other 2 tracks and internal

	$scope.selectTurn(0);
}]);
