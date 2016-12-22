"use strict";

angular.module("spaceAlertModule")
.controller("ResolutionController", ["$scope", "gameData", function ($scope, gameData) {
	$scope.gameData = gameData;

	$scope.selectPhase = function (phase) {
		$scope.currentPhase = phase;
	}
	$scope.selectTurn = function (turn) {
		$scope.currentTurn = turn;
		$scope.selectPhase(0);
	}

	//TODO: Fix lines when threats scroll

	$scope.selectTurn(0);
}])
.directive('threatTrack', function() {
	return {
		templateUrl: 'Content/Templates/ThreatTrack.html',
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
					return threatElement.offset().top + (threatElement.height() / 2);
				return 0;
			}

			$scope.getTrackSpaceCornerX = function (threat) {
				var spaceElement = $('#space' + $scope.trackId + threat.position);
				if (spaceElement.offset())
					return spaceElement.offset().left + spaceElement.width();
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
});
