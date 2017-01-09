﻿"use strict";

var cloneThreat = function(threat) {
	return {
		threatType: threat.threatType,
		threatDifficulty: threat.threatDifficulty,
		position: threat.position,
		remainingHealth: threat.remainingHealth,
		speed: threat.speed,
		description: threat.description,
		timeAppears: threat.timeAppears,
		id: threat.id,
		name: threat.name,

		shields: threat.shields,
		currentZone: threat.currentZone,

		totalInaccessibility: threat.totalInaccessibility,
		currentStations: threat.currentStations
	};
};

angular.module("spaceAlertModule")
	.controller('ThreatsModalInstanceCtrl',
	[
		'$uibModalInstance', '$scope', 'currentThreats', 'allThreats', 'allUsedThreats', 'zone',
		function($uibModalInstance, $scope, currentThreats, allThreats, allUsedThreats, zone) {
			$scope.currentThreats = currentThreats;
			$scope.allTimes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
			$scope.allThreats = allThreats;
			$scope.zone = zone;
			$scope.allUsedThreats = allUsedThreats;

			$scope.$watch('threatsGroupedByType',
				function(newValue) {
					$scope.threatsToChooseFrom = newValue.seriousThreats;
				});
			$scope.$watch('threatsToChooseFrom',
				function() {
					$scope.selectedThreatToAdd = null;
					$scope.selectedTimeOfThreatToAdd = null;
				});
			$scope.threatsGroupedByType = allThreats.whiteThreats;

			$scope.selectThreatToAdd = function(threat) {
				$scope.selectedThreatToAdd = threat;
			}

			$scope.getAvailableThreats = function() {
				return _.differenceBy($scope.threatsToChooseFrom, $scope.allUsedThreats, 'id');
			}

			$scope.ok = function() {
				var newThreat = cloneThreat($scope.selectedThreatToAdd);
				newThreat.timeAppears = $scope.selectedTimeOfThreatToAdd;
				$uibModalInstance.close(newThreat);
			};

			$scope.cancel = function() {
				$uibModalInstance.dismiss('cancel');
			};
		}
	]);