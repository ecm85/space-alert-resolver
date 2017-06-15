"use strict";

angular.module("spaceAlertModule")
    .directive('bonusThreatEntry',
        function () {
            return {
                templateUrl: 'templates/bonusThreatEntry',
                restrict: 'E',
                scope: {
                    threat: '=',
                    bonusThreat: '=',
                    setBonusThreat: '&',
                    clearBonusThreat: '&'
                },
                controller: [
                    '$scope',
                    function BonusThreatEntryController($scope) {
                        $scope.removeThreat = function() {
                            $scope.clearBonusThreat();
                        }
                    }
                ]
            };
        });

