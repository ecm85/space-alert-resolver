"use strict";

angular.module("spaceAlertModule")
    .controller("ReportBugController",
    [
        "$scope", "newGameData", '$location', '$http', function ($scope, newGameData, $location, $http) {
            const gameData = newGameData.manualData || newGameData.getGameArgs();
            $scope.includeGameData = !!(gameData);
            $scope.submitBug = function () {
                var data = '';

                const includeProblem = $scope.problem != null;
                if (includeProblem)
                    data += 'User report: ' + $scope.problem + '\r\n';

                if ($scope.includeGameData)
                    data += "Game data: " + JSON.stringify(gameData);

                const anyDataToInclude = $scope.includeGameData || includeProblem;
                if (anyDataToInclude) {
                    const includeEmail = $scope.email != null;
                    $http({
                        url: 'SendGameMessage?senderEmailAddress=' + (includeEmail ? $scope.email : ''),
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
