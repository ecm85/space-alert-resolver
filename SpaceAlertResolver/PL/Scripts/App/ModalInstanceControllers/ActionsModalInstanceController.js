"use strict";

var cloneAction = function(action) {
    return {
        hotkey: action.hotkey,
        displayText: action.displayText,
        description: action.description,
        firstAction: action.firstAction,
        secondAction: action.secondAction
    };
};

angular.module("spaceAlertModule")
    .controller('ActionsModalInstanceCtrl',
    [
        '$uibModalInstance', '$scope', 'player', 'allSingleActions', 'allDoubleActions', 'useDoubleActions', 'playerSpecializationActions', 'useSpecializations', 'hotkeys',
        function ($uibModalInstance, $scope, player, allSingleActions, allDoubleActions, useDoubleActions, playerSpecializationActions, useSpecializations, hotkeys) {
            $scope.useDoubleActions = useDoubleActions;
            $scope.useSpecializations = useSpecializations;
            $scope.allSingleActions = allSingleActions;
            $scope.allDoubleActions = allDoubleActions;
            $scope.playerSpecializationActions = playerSpecializationActions;
            $scope.selectedActions = player.actions.slice();
            $scope.playerColor = player.color.model;
            $scope.playerTitle = player.title;
            $scope.cursor = { index: 0 };

            $scope.allSingleActions.concat($scope.playerSpecializationActions).forEach(function (action) {
                if (action.hotkey)
                    hotkeys.bindTo($scope)
                        .add({
                            combo: action.hotkey,
                            description: action.displayText,
                            callback: function() { $scope.addActionAtCursor(action); }
                        });
            });

            $scope.addActionAtCursor = function (action) {
                $scope.cursor.errorMessage = null;
                if ($scope.cursor.index < 12) {
                    if (($scope.cursor.actionIndex || 0) === 0) {
                        $scope.selectedActions[$scope.cursor.index] = cloneAction(action);
                        const canAddSecondAction = $scope.useDoubleActions &&
                            action.secondAction == null
                            && _.some($scope.allDoubleActions, function (doubleAction) { return doubleAction.firstAction === action.firstAction }); 
                        if (canAddSecondAction){
                            $scope.cursor.actionIndex = 1;
                        } else {
                            $scope.cursor.index++;
                            $scope.cursor.actionIndex = 0;
                        }
                    } else {
                        if (action.firstAction === null) {
                            $scope.cursor.index++;
                            $scope.cursor.actionIndex = 0;
                        } else {
                            var firstActionOfDoubleAction = $scope.selectedActions[$scope.cursor.index].firstAction;
                            var secondActionOfDoubleAction = action.firstAction;
                            var selectedDoubleAction = _.find($scope.allDoubleActions,
                                function(doubleAction) {
                                    return doubleAction.firstAction === firstActionOfDoubleAction &&
                                        doubleAction.secondAction === secondActionOfDoubleAction;
                                });
                            if (selectedDoubleAction) {
                                $scope.selectedActions[$scope.cursor.index] = cloneAction(selectedDoubleAction);
                                $scope.cursor.index++;
                                $scope.cursor.actionIndex = 0;
                            } else {
                                $scope.cursor.errorMessage = 'Invalid double action: ' + $scope.selectedActions[$scope.cursor.index].hotkey + ' - ' + action.hotkey;
                            }
                        }
                    }
                }

                //TODO: Do something otherwise?
                if ($scope.cursor.index=== 12)
                    $scope.cursor.index = 0;
            }

            $scope.addBonusActionAtCursor = function(action) {
                $scope.selectedActions[$scope.cursor.index].bonusAction = action;
            }

            $scope.ok = function() {
                player.actions = $scope.selectedActions;
                $uibModalInstance.close();
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        }
    ]);
