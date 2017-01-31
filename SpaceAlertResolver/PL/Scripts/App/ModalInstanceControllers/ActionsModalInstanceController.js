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
		'$uibModalInstance', '$scope', 'player', 'allSingleActions', 'allDoubleActions', 'hotkeys',
		function ($uibModalInstance, $scope, player, allSingleActions, allDoubleActions, hotkeys) {
			$scope.allSingleActions = allSingleActions;
			$scope.allDoubleActions = allDoubleActions;
			$scope.selectedActions = player.actions.slice();
			$scope.playerColor = player.color.model;
			$scope.playerTitle = player.title;
			$scope.cursor = { index: 0 };

			$scope.allSingleActions.concat($scope.allDoubleActions).forEach(function (action) {
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
					$scope.selectedActions[$scope.cursor.index] = cloneAction(action);
					$scope.cursor.index++;
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
