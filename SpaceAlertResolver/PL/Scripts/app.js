"use strict";

var cloneAction = function(action) {
	return {
		hotkey: action.hotkey,
		displayText: action.displayText,
		description: action.description,
		action: action.action
	};
}
var cloneThreat = function(threat) {
	return {
		threatType: threat.threatType,
		threatDifficulty: threat.threatDifficulty,
		position: threat.position,
		remainingHealth: threat.remainingHealth,
		speed: threat.speed,
		description: threat.description,
		timeAppears: threat.timeAppears,
		id: threat.id,

		shields: threat.shields,
		currentZone: threat.currentZone,
		
		totalInaccessibility: threat.totalInaccessibility,
		currentStations: threat.currentStations
	};
}
angular.module("spaceAlertModule")
.controller("ResolutionController", ["$scope", "gameData", '$interval', function ($scope, gameData, $interval) {
	$scope.gameData = gameData;
	$scope.playing = null;
	$scope.turnsPerTwoSeconds = 5;
	var selectPhase = function (phase) {
		$scope.currentPhase = phase;
	}
	var selectTurn = function (turn) {
		$scope.currentTurn = turn;
		selectPhase(0);
	}
	var stop = function () {
		if ($scope.playing != null) {
			$interval.cancel($scope.playing);
			$scope.playing = null;
		}
	};
	var play = function () {
		$scope.playing = $interval(function () {
			if (!$scope.isAtEndOfTurn())
				$scope.selectPhaseAutomatically($scope.currentPhase + 1);
			else if (!$scope.isAtEndOfGame())
				$scope.selectTurnAutomatically($scope.currentTurn + 1);
			else {
				stop();
			}
		},
			2000 / $scope.turnsPerTwoSeconds);
	};
	$scope.$watch('turnsPerTwoSeconds', function() {
		if ($scope.playing != null){
			stop();
			play();
		}
	});

	$scope.selectTurnManually = function (turn) {
		stop();
		selectTurn(turn);
	}
	$scope.selectTurnAutomatically = function(turn) {
		selectTurn(turn);
	}

	$scope.selectPhaseManually = function(phase) {
		stop();
		selectPhase(phase);
	}
	$scope.selectPhaseAutomatically = function (phase) {
		selectPhase(phase);
	}

	$scope.isAtStartOfTurn = function() {
		return $scope.currentPhase === 0;
	}
	$scope.isAtStartOfGame = function() {
		return $scope.currentTurn === 0 && $scope.isAtStartOfTurn();
	}
	$scope.isAtEndOfTurn = function() {
		return $scope.currentPhase === $scope.gameData[$scope.currentTurn].length - 1;
	}
	$scope.isAtEndOfGame = function() {
		return $scope.currentTurn === $scope.gameData.length - 1 && $scope.isAtEndOfTurn();
	}
	$scope.goBackOnePhase = function() {
		if (!$scope.isAtStartOfTurn())
			$scope.selectPhaseManually($scope.currentPhase - 1);
		else if (!$scope.isAtStartOfGame()) {
			$scope.selectTurnManually($scope.currentTurn - 1);
			$scope.selectPhaseManually($scope.gameData[$scope.currentTurn].length - 1);
		}
	}
	$scope.goForwardOnePhase = function() {
		if (!$scope.isAtEndOfTurn())
			$scope.selectPhaseManually($scope.currentPhase + 1);
		else if (!$scope.isAtEndOfGame()) {
			$scope.selectTurnManually($scope.currentTurn + 1);
		}
	}
	$scope.playPause = function() {
		if ($scope.playing)
			stop();
		else
			play();
	}
	$scope.goToStart = function() {
		if (!$scope.isAtStartOfGame()) {
			stop();
			$scope.selectTurnManually(0);
		}
	}
	$scope.goToEnd = function () {
		if (!$scope.isAtEndOfGame()) {
			stop();
			$scope.selectTurnManually($scope.gameData.length - 1);
			$scope.selectPhaseManually($scope.gameData[$scope.currentTurn].length - 1);
		}
	}
	$scope.$on('$destroy', function() {
		stop();
	});

	$scope.selectTurnManually(0);
}])
.directive('trackWithThreats', function() {
	return {
		templateUrl: 'templates/trackWithThreats',
		restrict: 'E',
		scope: {
			track: '=',
			trackId: '=',
			zoneDescription: '=',
			allTracks: '=',
			allUsedTracks: '=',
			trackIsConfigurable: '=',
			threats: '=',
			allThreats: '=',
			allUsedThreats: '=',
			threatsAreConfigurable: '='
		},
		controller: ['$scope', '$uibModal', function TrackWithThreatsController($scope, $uibModal) {
			$scope.getThreatCornerX = function (index) {
				var threatElement = $('#threat' + $scope.trackId + index);
				if (threatElement.offset())
					return threatElement.offset().left;
				return 0;
			}
			$scope.getThreatCornerY = function (index) {
				var threatElement = $('#threat' + $scope.trackId + index);
				if (threatElement.offset())
					return threatElement.offset().top;
				return 0;
			}
			$scope.getTrackSpaceCornerX = function (threat) {
				var spaceElement = $('#space' + $scope.trackId + threat.position);
				if (spaceElement.offset())
					return spaceElement.offset().left + spaceElement.outerWidth() - 1;
				return 0;
			}
			$scope.getTrackSpaceCornerY = function (threat) {
				var spaceElement = $('#space' + $scope.trackId + threat.position);
				if (spaceElement.offset())
					return spaceElement.offset().top;
				return 0;
			}
			$scope.getStationCornerX = function(threat, station) {
				var stationElement = $('#' + station.toLowerCase() + 'threats');
				if (stationElement.offset())
					return stationElement.offset().left;
				return 0;
			}
			$scope.getStationCornerY = function (threat, station) {
				var stationElement = $('#' + station.toLowerCase() + 'threats');
				if (stationElement.offset())
					return stationElement.offset().top;
				return 0;
			}

			$scope.configureTrack = function () {
				if (!$scope.trackIsConfigurable)
					return;
				var modal = $uibModal.open({
					animation: true,
					templateUrl: 'templates/trackModal',
					controller: 'TrackModalInstanceCtrl',
					size: 'lg',
					resolve: {
						currentTrack: function () {
							return $scope.track;
						},
						allTracks: function () {
							return $scope.allTracks;
						},
						zone: function () {
							return $scope.zoneDescription;
						},
						usedTracks: function () {
							return $scope.allUsedTracks;
						}
					}
				});
				modal.result.then(function (selectedTrack) {
					$scope.track = selectedTrack;
				});
			}

			$scope.addNewThreat = function () {
				if (!$scope.threatsAreConfigurable)
					return;
				var modal = $uibModal.open({
					animation: true,
					templateUrl: 'templates/threatsModal',
					controller: 'ThreatsModalInstanceCtrl',
					size: 'lg',
					resolve: {
						currentThreats: function () {
							return $scope.threats;
						},
						allThreats: function () {
							return $scope.allThreats;
						},
						allUsedThreats: function() {
							return $scope.allUsedThreats;
						},
						zone: function () {
							return $scope.zoneDescription;
						}
					}
				});
				modal.result.then(function (threat) {
					$scope.threats.push(threat);
				});
			}

			$scope.showThreats = function() {
				return $scope.threats || $scope.threatsAreConfigurable;
			}

			$scope.canAddNewThreat = function() {
				return $scope.threatsAreConfigurable && $scope.threats.length < 3;
			}

			//TODO:
			//$scope.removeThreat = function (threat) {
			//	$scope.selectedThreats.splice($scope.selectedThreats.indexOf(threat), 1);
			//}
		}]
	}
})
.directive('standardZone', function () {
	return {
		templateUrl: 'templates/standardZone',
		restrict: 'E',
		scope: {
			zone: '=',
			zoneId: '='
		}
	}
})
.directive('threat', function () {
	return {
		templateUrl: 'templates/threat',
		restrict: 'E',
		scope: {
			threatIndex: '=',
			threat: '=',
			trackId: '='
		}
	};
})
.directive('trackSpaces', function() {
	return {
		templateUrl: 'templates/trackSpaces',
		retrict: 'E',
		scope: {
			track: '=',
			trackId: '='
		}
	};
})
.controller("InputController", ["$scope", '$uibModal', 'inputData', function ($scope, $uibModal, inputData) {
	$scope.allTracks = inputData.tracks;
	$scope.selectedTracks = {
		redTrack: null,
		whiteTrack: null,
		blueTrack: null,
		internalTrack: null
	};
	var updateAllSelectedTracks = function() {
		$scope.allSelectedTracks = [
			$scope.selectedTracks.redTrack,
			$scope.selectedTracks.whiteTrack,
			$scope.selectedTracks.blueTrack,
			$scope.selectedTracks.internalTrack
		];
	}
	updateAllSelectedTracks();
	$scope.$watch('selectedTracks.redTrack', function () {
		updateAllSelectedTracks();
	});
	$scope.$watch('selectedTracks.whiteTrack', function () {
		updateAllSelectedTracks();
	});
	$scope.$watch('selectedTracks.blueTrack', function () {
		updateAllSelectedTracks();
	});
	$scope.$watch('selectedTracks.internalTrack', function () {
		updateAllSelectedTracks();
	});

	var checkDuplicateRedTrack = function (track) {
		if ($scope.selectedTracks.redTrack === track)
			$scope.selectedTracks.redTrack = null;
	}
	var checkDuplicateWhiteTrack = function (track) {
		if ($scope.selectedTracks.whiteTrack === track)
			$scope.selectedTracks.whiteTrack = null;
	}
	var checkDuplicateBlueTrack = function (track) {
		if ($scope.selectedTracks.blueTrack === track)
			$scope.selectedTracks.blueTrack = null;
	}
	var checkDuplicateInternalTrack = function (track) {
		if ($scope.selectedTracks.internalTrack === track)
			$scope.selectedTracks.internalTrack = null;
	}
	$scope.$watch('selectedTracks.redTrack',
		function (newValue) {
			if (!newValue)
				return;
			checkDuplicateWhiteTrack(newValue);
			checkDuplicateBlueTrack(newValue);
			checkDuplicateInternalTrack(newValue);
		});
	$scope.$watch('selectedTracks.whiteTrack',
		function (newValue) {
			if (!newValue)
				return;
			checkDuplicateRedTrack(newValue);
			checkDuplicateBlueTrack(newValue);
			checkDuplicateInternalTrack(newValue);
		});
	$scope.$watch('selectedTracks.blueTrack',
		function (newValue) {
			if (!newValue)
				return;
			checkDuplicateRedTrack(newValue);
			checkDuplicateWhiteTrack(newValue);
			checkDuplicateInternalTrack(newValue);
		});
	$scope.$watch('selectedTracks.internalTrack',
		function (newValue) {
			if (!newValue)
				return;
			checkDuplicateRedTrack(newValue);
			checkDuplicateWhiteTrack(newValue);
			checkDuplicateBlueTrack(newValue);
		});

	$scope.allInternalThreats = inputData.allInternalThreats;
	$scope.allExternalThreats = inputData.allExternalThreats;
	$scope.selectedThreats = {
		redThreats: [],
		whiteThreats: [],
		blueThreats: [],
		internalThreats: []
	}
	var updateAllSelectedExternalThreats = function() {
		$scope.allSelectedExternalThreats = $scope.selectedThreats.redThreats.concat($scope.selectedThreats.whiteThreats).concat($scope.selectedThreats.blueThreats);
	}
	$scope.$watchCollection('selectedThreats.redThreats', function () {
		updateAllSelectedExternalThreats();
	});
	$scope.$watchCollection('selectedThreats.whiteThreats', function () {
		updateAllSelectedExternalThreats();
	});
	$scope.$watchCollection('selectedThreats.blueThreats', function () {
		updateAllSelectedExternalThreats();
	});

	//TODO: Add specializations
	//TODO: Add double actions

	$scope.allActions = inputData.actions;

	$scope.colors = ['blue', 'green', 'red', 'yellow', 'purple'];
	$scope.playerCounts = [1, 2, 3, 4, 5];
	$scope.players = [
		{ title: 'Captain', color: { model: $scope.colors[0] }, actions: [] },
		{ title: 'Player 2', color: { model: $scope.colors[1] }, actions: [] },
		{ title: 'Player 3', color: { model: $scope.colors[2] }, actions: [] },
		{ title: 'Player 4', color: { model: $scope.colors[3] }, actions: [] },
		{ title: 'Player 5', color: { model: $scope.colors[4] }, actions: [] }
	];
	$scope.players.forEach(function (player) {
		for (var i = 0; i < 12; i++)
			player.actions.push(cloneAction($scope.allActions[0]));
	});
	$scope.selectPlayerCount = function (newPlayerCount) {
		$scope.selectedPlayerCountRadio = { model: newPlayerCount };
		$scope.players.forEach(function(player, index) {
			player.isInGame = index < newPlayerCount;
		});
	}
	$scope.selectPlayerCount(4);
	$scope.$watch('selectedPlayerCountRadio.model', function(newPlayerCount) {
		$scope.selectPlayerCount(newPlayerCount);
	});
	$scope.players.forEach(function (player, index) {
		$scope.$watch(
			function (scope) {
				return scope.players[index].color.model;
			},
			function (newValue, oldValue) {
				$scope.players.forEach(function (player, exisitingPlayerIndex) {
					if (index !== exisitingPlayerIndex && player.color.model === newValue)
						player.color.model = oldValue;
				});
			});
	});

	$scope.animationsEnabled = true;

	$scope.openActionsDialog = function (player, size) {
		$uibModal.open({
			animation: true,
			templateUrl: 'templates/actionsModal',
			controller: 'ActionsModalInstanceCtrl',
			size: size,
			resolve: {
				player: function() {
					return player;
				},
				allActions: function() {
					return $scope.allActions;
				}
			}
		});
	};

	$scope.canCreateGame = function() {
		return $scope.selectedTracks.redTrack != null && $scope.selectedTracks.whiteTrack != null && $scope.selectedTracks.blueTrack != null && $scope.selectedTracks.internalTrack != null;
	}

	var getColorIndex = function(playerColor) {
		for(var i = 0; i < $scope.colors.length; i++)
			if ($scope.colors[i] === playerColor)
				return i;
		return -1;
	}
	$scope.getGameArgs = function () {
		if (!$scope.canCreateGame())
			return '';
		var players = [];
		for (var i = 0; i < $scope.players.length; i++) {
			var player = $scope.players[i];
			if (player.isInGame)
				players.push({
					actions: player.actions,
					index: i,
					playerColor: getColorIndex(player.color.model)
				});
		}
		var game = {
			players: players,
			redThreats: $scope.selectedThreats.redThreats,
			whiteThreats: $scope.selectedThreats.whiteThreats,
			blueThreats: $scope.selectedThreats.blueThreats,
			internalThreats: $scope.selectedThreats.internalThreats,
			redTrack: $scope.selectedTracks.redTrack,
			whiteTrack: $scope.selectedTracks.whiteTrack,
			blueTrack: $scope.selectedTracks.blueTrack,
			internalTrack: $scope.selectedTracks.internalTrack
		};
		return JSON.stringify(game);
	}
}])
.controller('ActionsModalInstanceCtrl', ['$uibModalInstance', '$scope', 'player', 'allActions', 'hotkeys', function ($uibModalInstance, $scope, player, allActions, hotkeys) {
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
			callback: function () { $scope.addActionAtCursor(action); }
		});
	});

	$scope.addActionAtCursor = function (action) {
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

	$scope.ok = function () {
		player.actions = $scope.selectedActions;
		$uibModalInstance.close();
	};

	$scope.cancel = function () {
		$uibModalInstance.dismiss('cancel');
	};
}])
.controller('TrackModalInstanceCtrl', ['$uibModalInstance', '$scope', 'currentTrack', 'allTracks', 'zone', 'usedTracks', function ($uibModalInstance, $scope, currentTrack, allTracks, zone, usedTracks) {
	$scope.selectedTrack = currentTrack;
	$scope.allTracks = allTracks;
	$scope.zone = zone;
	$scope.usedTracks = usedTracks;

	$scope.trackIsInUse = function(track) {
		if ($scope.selectedTrack === track)
			return false;
		for(var i = 0; i < $scope.usedTracks.length; i++)
			if ($scope.usedTracks[i] === track)
				return true;
		return false;
	}

	$scope.selectTrack = function (track) {
		$scope.selectedTrack = track;
		$uibModalInstance.close($scope.selectedTrack);
	}

	$scope.cancel = function () {
		$uibModalInstance.dismiss('cancel');
	};
}])
.controller('ThreatsModalInstanceCtrl', ['$uibModalInstance', '$scope', 'currentThreats', 'allThreats', 'allUsedThreats', 'zone', function ($uibModalInstance, $scope, currentThreats, allThreats, allUsedThreats, zone) {
	$scope.currentThreats = currentThreats;
	$scope.allTimes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
	$scope.allThreats = allThreats;
	$scope.zone = zone;
	$scope.allUsedThreats = allUsedThreats;

	$scope.$watch('threatsGroupedByType', function(newValue) {
		$scope.threatsToChooseFrom = newValue.seriousThreats;
	});
	$scope.$watch('threatsToChooseFrom', function () {
		$scope.selectedThreatToAdd = null;
		$scope.selectedTimeOfThreatToAdd = null;
	});
	$scope.threatsGroupedByType = allThreats.whiteThreats;

	$scope.selectThreatToAdd = function(threat) {
		$scope.selectedThreatToAdd = threat;
	}
	
	$scope.getAvailableThreats = function()
	{
		return _.differenceBy($scope.threatsToChooseFrom, $scope.allUsedThreats, 'id');
	}

	$scope.ok = function () {
		var newThreat = cloneThreat($scope.selectedThreatToAdd);
		newThreat.timeAppears = $scope.selectedTimeOfThreatToAdd;
		$uibModalInstance.close(newThreat);
	};

	$scope.cancel = function () {
		$uibModalInstance.dismiss('cancel');
	};
}]);
