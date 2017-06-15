"use strict";

angular.module("spaceAlertModule")
    .controller("ManualController",
    [
        "$scope", 'newGameData', function($scope, newGameData) {
            $scope.newGameData = newGameData;
        }
    ]);
