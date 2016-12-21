"use strict";

angular.module("spaceAlertModule").controller("ResolutionController", ["$scope", "gameData", "$timeout", function ($scope, gameData, $timeout) {
	$scope.gameData = gameData;

	$scope.selectPhase = function (phase) {
		$scope.currentPhase = phase;
		$scope.updateThreatLines();
	}
	$scope.selectTurn = function (turn) {
		$scope.currentTurn = turn;
		$scope.selectPhase(0);
	}


	//$scope.getThreatCornerX = function(color, index) {
	//	return $(color + 'Threat' + index).position().left;
	//}

	//$scope.getThreatCornerY = function (color, index) {
	//	return $(color + 'Threat' + index).position().top;
	//}

	//$scope.getTrackSpaceCornerX = function(color, threat) {
	//	return $(color + 'Space' + threat.position).position().top;
	//}

	//$scope.getTrackSpaceCornerY = function (color, threat) {
	//	return $(color + 'Space' + threat.position).position().left;
	//}
	

	//TODO: Try to change these back from a timeout to functions (and correctly use #, dummy)
	//TODO: Check what happens when threats scroll
	//TODO: Add to other 2 tracks and internal

	$scope.updateThreatLines = function() {
		$timeout(function() {
			gameData[$scope.currentTurn][$scope.currentPhase].redThreats.forEach(function (redThreat, index) {
				var readThreatLineElement = $('#redThreatLine' + index);
				var threatElement = $('#redThreat' + index);
				var trackSpaceElement = $('#redSpace' + redThreat.position);
				var threatPosition = threatElement.offset();
				var trackSpacePosition = trackSpaceElement.offset();
				var trackSpaceWidth = trackSpaceElement.width();
				var threatHeight = threatElement.height();
				var x1 = threatPosition.left;
				var y1 = threatPosition.top + (threatHeight / 2);
				var x2 = trackSpacePosition.left + trackSpaceWidth;
				var y2 = trackSpacePosition.top;

				readThreatLineElement.attr('x1', x1).attr('y1', y1).attr('x2', x2).attr('y2', y2);
				trackSpaceElement.addClass('threat-present');
			}, this);
		});
	}

	$scope.selectTurn(0);
}]);


//ng-attr-x1="{{getThreatCornerX('red', $index)}}"
//ng-attr-y1="{{getThreatCornerY('red', $index)}}"
//ng-attr-x2="{{getTrackSpaceCornerX('red', externalThreat)}}"
//ng-attr-y2="{{getTrackSpaceCornerY('red', externalThreat)}}"
