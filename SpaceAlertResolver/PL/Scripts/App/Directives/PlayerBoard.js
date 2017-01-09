"use strict";

angular.module("spaceAlertModule")
	.directive('playerBoard',
		function() {
			return {
				templateUrl: 'templates/playerBoard',
				restrict: 'E',
				scope: {
					cursor: '=',
					selectedActions: '=',
					playerColor: '='
				},
				controller: [
					'$scope',
					function PlayerBoardController($scope) {
						$scope.moveCursor = function (index) {
							if ($scope.cursor)
								$scope.cursor.index = index;
						};
					}
				]
			};
		});

