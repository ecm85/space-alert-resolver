"use strict";

angular.module("spaceAlertModule")
    .directive('standardZone',
        function() {
            return {
                templateUrl: 'templates/standardZone',
                restrict: 'E',
                scope: {
                    zone: '=',
                    zoneId: '='
                }
            }
        });
