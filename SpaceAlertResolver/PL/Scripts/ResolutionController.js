"use strict";

angular.module("spaceAlertModule")
.controller("ResolutionController", ["$scope", "gameData", '$interval', function ($scope, gameData, $interval) {
	$scope.gameData = gameData;
	$scope.playing = null
	$scope.selectPhase = function (phase) {
		$scope.currentPhase = phase;
	}
	$scope.selectTurn = function (turn) {
		$scope.currentTurn = turn;
		$scope.selectPhase(0);
	}
	$scope.play = function() {
		$scope.playing = $interval(function() {
			if ($scope.currentPhase < $scope.gameData[$scope.currentTurn].length - 1)
				$scope.selectPhase($scope.currentPhase + 1);
			else if ($scope.currentTurn < $scope.gameData.length - 1)
				$scope.selectTurn($scope.currentTurn + 1);
			else {
				$scope.stop();
			}
		},
		400);
	}
	$scope.stop = function() {
		if ($scope.playing != null) {
			$interval.cancel($scope.playing);
			$scope.playing = null;
		}
	}
	$scope.goToStart = function() {
		$scope.stop();
		$scope.selectTurn(0);
	}
	$scope.goToEnd = function() {
		$scope.stop();
		$scope.selectTurn($scope.gameData.length - 1);
		$scope.selectPhase($scope.gameData[$scope.currentTurn].length - 1);
	}
	$scope.$on('$destroy', function() {
		$scope.stop();
	});
	//TODO: Fix lines when threats scroll

	$scope.selectTurn(0);
}])
.directive('threatTrack', function() {
	return {
		templateUrl: 'SpaceAlertResolver/Content/Templates/ThreatTrack.html',
		restrict: 'E',
		scope: {
			threats: '=',
			track: '=',
			trackId: '='
		},
		controller: ['$scope', function ThreatTrackController($scope) {
			$scope.getThreatCornerX = function (index) {
				var threatElement = $('#threat' + $scope.trackId + index);
				if (threatElement.offset())
					return threatElement.offset().left;
				return 0;
			}

			$scope.getThreatCornerY = function (index) {
				var threatElement = $('#threat' + $scope.trackId + index);
				if (threatElement.offset())
					return threatElement.offset().top + (threatElement.outerHeight() / 2);
				return 0;
			}

			$scope.getTrackSpaceCornerX = function (threat) {
				var spaceElement = $('#space' + $scope.trackId + threat.position);
				if (spaceElement.offset())
					return spaceElement.offset().left + spaceElement.outerWidth() - 1;
				return 0;
			}

			$scope.getTrackSpaceCornerY = function (threat) {
				var spaceElement = $('#space' + $scope.trackId + threat.position);
				if (spaceElement.offset())
					return spaceElement.offset().top;
				return 0;
			}
		}]
	}
})
.directive('standardZone', function () {
	return {
		templateUrl: 'SpaceAlertResolver/Content/Templates/StandardZone.html',
		restrict: 'E',
		scope: {
			zone: '=',
			zoneId: '='
		}
	}
});
