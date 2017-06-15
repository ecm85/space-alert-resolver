"use strict";

angular.module("spaceAlertModule")
    .directive('track',
        function() {
            return {
                templateUrl: 'templates/track',
                restrict: 'E',
                scope: {
                    track: '=',
                    trackId: '='
                }
            };
        });
