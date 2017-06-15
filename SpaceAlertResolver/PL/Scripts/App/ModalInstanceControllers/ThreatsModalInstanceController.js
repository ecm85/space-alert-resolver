"use strict";

var cloneThreat = function(threat) {
    return {
        threatType: threat.threatType,
        threatDifficulty: threat.threatDifficulty,
        position: threat.position,
        remainingHealth: threat.remainingHealth,
        speed: threat.speed,
        fileName: threat.fileName,
        timeAppears: threat.timeAppears,
        id: threat.id,
        displayName: threat.displayName,
        points: threat.points,
        buffCount: threat.buffCount,
        debuffCount: threat.debuffCount,
        needsBonusExternalThreat: threat.needsBonusExternalThreat,
        needsBonusInternalThreat: threat.needsBonusInternalThreat,
        bonusInternalThreat: threat.bonusInternalThreat,
        bonusExternalThreat: threat.bonusExternalThreat,

        shields: threat.shields,
        currentZone: threat.currentZone,

        totalInaccessibility: threat.totalInaccessibility,
        displayOnTrackStations: threat.displayOnTrackStations
    };
};

angular.module("spaceAlertModule")
    .controller('ThreatsModalInstanceCtrl',
    [
        '$uibModalInstance', '$scope', 'allThreats', 'allUsedThreats', 'zone', 'threatAppearsNormally',
        function ($uibModalInstance, $scope, allThreats, allUsedThreats, zone, threatAppearsNormally) {
            $scope.threatAppearsNormally = threatAppearsNormally;
            $scope.allTimes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
            $scope.allThreats = allThreats;
            $scope.zone = zone;
            $scope.allUsedThreats = allUsedThreats;

            $scope.$watch('threatsGroupedByType',
                function(newValue) {
                    $scope.threatsToChooseFrom = newValue.minorThreats;
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
