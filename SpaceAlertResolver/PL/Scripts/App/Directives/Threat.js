"use strict";

angular.module("spaceAlertModule")
    .directive('threat',
        function() {
            return {
                templateUrl: 'templates/threat',
                restrict: 'E',
                scope: {
                    threatIndex: '=',
                    threat: '=',
                    trackId: '=',
                    removable: '=',
                    removeThreat: '&?'
                }
            };
        });
