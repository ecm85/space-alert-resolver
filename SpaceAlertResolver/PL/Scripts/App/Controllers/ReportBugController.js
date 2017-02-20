"use strict";

angular.module("spaceAlertModule")
	.controller("ReportBugController",
	[
		"$scope", "newGameData", '$location', '$http', function ($scope, newGameData, $location, $http) {
			$scope.canIncludeGameData = true && (newGameData.manualData || newGameData.getGameArgs());
			$scope.includeGameData = $scope.canIncludeGameData;
			$scope.submitBug = function () {
				var data = '';
				if ($scope.problem != null)
					data += 'User report: ' + $scope.problem + '\r\n';
				var gameData = newGameData.manualData || newGameData.getGameArgs();
				if (gameData)
					data += "Game data: " + JSON.stringify(gameData);
				if (gameData || $scope.problem != null || $scope.email != null) {
					$http({
						url: 'SendGameMessage?senderEmailAddress=' + ($scope.email != null ? $scope.email : ''),
						method: "POST",
						data: {
							messageText: data
						},
						headers: { 'Content-Type': 'application/json' }
					});
					$location.path('/#');
				}
			}
		}
	]);
