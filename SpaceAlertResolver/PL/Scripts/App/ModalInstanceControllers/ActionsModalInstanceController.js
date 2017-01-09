"use strict";

var cloneAction = function(action) {
	return {
		hotkey: action.hotkey,
		displayText: action.displayText,
		description: action.description,
		action: action.action
	};
};

angular.module("spaceAlertModule")
	.controller('ActionsModalInstanceCtrl',
	[
		'$uibModalInstance', '$scope', 'player', 'allActions', 'hotkeys',
		function($uibModalInstance, $scope, player, allActions, hotkeys) {
			$scope.allActions = allActions;
			$scope.selectedActions = player.actions.slice();
			$scope.playerColor = player.color.model;
			$scope.playerTitle = player.title;
			$scope.cursor = 0;

			$scope.allActions.forEach(function(action) {
				hotkeys.bindTo($scope)
					.add({
						combo: action.hotkey,
						description: action.displayText,
						callback: function() { $scope.addActionAtCursor(action); }
					});
			});

			$scope.addActionAtCursor = function(action) {
				if ($scope.cursor < 12) {
					$scope.selectedActions[$scope.cursor] = cloneAction(action);
					$scope.cursor++;
				}
				//TODO: Do something otherwise?
				if ($scope.cursor === 12)
					$scope.cursor = 0;
			}

			$scope.moveCursor = function(index) {
				$scope.cursor = index;
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
