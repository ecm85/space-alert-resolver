"use strict";

angular.module("spaceAlertModule")
    .directive('playerBoard',
        function() {
            return {
                templateUrl: 'templates/playerBoard',
                restrict: 'E',
                scope: {
                    immutableCursor: '=',
                    cursor: '=',
                    selectedActions: '=',
                    playerColor: '=',
                    smallBoard: '='
                },
                controller: [
                    '$scope',
                    function PlayerBoardController($scope) {
                        $scope.moveCursor = function (index) {
                            if ($scope.cursor && !$scope.immutableCursor) {
                                $scope.cursor.index = index;
                                $scope.cursor.actionIndex = 0;
                                $scope.cursor.errorMessage = null;
                            }
                        };
                    }
                ]
            };
        });

