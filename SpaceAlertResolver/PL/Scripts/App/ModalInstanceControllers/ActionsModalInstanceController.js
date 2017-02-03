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
		'$uibModalInstance', '$scope', 'player', 'allSingleActions', 'allDoubleActions', 'useDoubleActions', 'hotkeys',
		function ($uibModalInstance, $scope, player, allSingleActions, allDoubleActions, useDoubleActions, hotkeys) {
			$scope.useDoubleActions = useDoubleActions;
			$scope.allSingleActions = allSingleActions;
			$scope.allDoubleActions = allDoubleActions;
			$scope.selectedActions = player.actions.slice();
			$scope.playerColor = player.color.model;
			$scope.playerTitle = player.title;
			$scope.cursor = { index: 0 };

			$scope.allSingleActions.forEach(function (action) {
				if (action.hotkey)
					hotkeys.bindTo($scope)
						.add({
							combo: action.hotkey,
							description: action.displayText,
							callback: function() { $scope.addActionAtCursor(action); }
						});
			});

			$scope.addActionAtCursor = function(action) {
				if ($scope.cursor.index < 12) {
					if (($scope.cursor.actionIndex || 0) === 0) {
						$scope.selectedActions[$scope.cursor.index] = cloneAction(action);
						if ($scope.useDoubleActions && _.some($scope.allDoubleActions, function(doubleAction){return doubleAction.firstAction === action.firstAction})) {
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
								//TODO: Error message
							}
						}
					}
				}
				//TODO: Do something otherwise?
				if ($scope.cursor.index=== 12)
					$scope.cursor.index = 0;
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
