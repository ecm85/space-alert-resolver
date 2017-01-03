"use strict";

var cloneAction = function(action) {
	return {
		serializationText: action.serializationText,
		displayText: action.displayText,
		entryText: action.entryText,
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
.directive('threatTrack', function() {
	return {
		templateUrl: 'templates/threatTrack',
		restrict: 'E',
		scope: {
			threats: '=',
			track: '=',
			trackId: '=',
			onTrackClicked: '&?'
		},
		controller: ['$scope', function ThreatTrackController($scope) {
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
	}
})
.controller("InputController", ["$scope", '$uibModal', 'inputData', function ($scope, $uibModal, inputData) {

	$scope.allTracks = inputData.tracks;
	$scope.allActions = inputData.actions;
	$scope.allInternalThreats = inputData.allInternalThreats;
	$scope.allExternalThreats = inputData.allExternalThreats;
	//TODO: Add specializations
	//TODO: Add double actions

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

	$scope.dropdownStatus = {
		isopen: false
	};

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

	$scope.items = ['item1', 'item2', 'item3'];

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

	var openTrackDialog = function (size, currentTrack, zone, trackSetterFn) {
		var modal = $uibModal.open({
			animation: true,
			templateUrl: 'templates/trackModal',
			controller: 'TrackModalInstanceCtrl',
			size: size,
			resolve: {
				currentTrack: function () {
					return currentTrack;
				},
				allTracks: function() {
					return $scope.allTracks;
				},
				zone: function() {
					return zone;
				}
			}
		});
		modal.result.then(function(selectedTrack) {
			trackSetterFn(selectedTrack);
		});
	}

	$scope.openRedTrackDialog = function() {
		openTrackDialog('lg', $scope.redTrack, 'Red', function(selectedTrack) { $scope.redTrack = selectedTrack; });
	}
	$scope.openWhiteTrackDialog = function () {
		openTrackDialog('lg', $scope.whiteTrack, 'White', function (selectedTrack) { $scope.whiteTrack = selectedTrack; });
	}
	$scope.openBlueTrackDialog = function () {
		openTrackDialog('lg', $scope.blueTrack, 'Blue', function (selectedTrack) { $scope.blueTrack = selectedTrack; });
	}
	$scope.openInternalTrackDialog = function () {
		openTrackDialog('lg', $scope.internalTrack, 'Internal', function (selectedTrack) { $scope.internalTrack = selectedTrack; });
	}

	$scope.redThreats = [];
	$scope.whiteThreats = [];
	$scope.blueThreats = [];
	$scope.internalThreats = [];

	var openThreatsDialog = function(size, currentThreats, allThreats, zone, threatsSetterFn) {
		var modal = $uibModal.open({
			animation: true,
			templateUrl: 'templates/threatsModal',
			controller: 'ThreatsModalInstanceCtrl',
			size: size,
			resolve: {
				currentThreats: function () {
					return currentThreats;
				},
				allThreats: function () {
					return allThreats;
				},
				zone: function () {
					return zone;
				}
			}
		});
		modal.result.then(function (threats) {
			threatsSetterFn(threats);
		});
	}

	$scope.openRedThreatsDialog = function (size) {
		openThreatsDialog(size, $scope.redThreats, $scope.allExternalThreats, 'Red', function (threats) { $scope.redThreats = threats; });
	}
	$scope.openWhiteThreatsDialog = function (size) {
		openThreatsDialog(size, $scope.whiteThreats, $scope.allExternalThreats, 'White', function (threats) { $scope.whiteThreats = threats; });
	}
	$scope.openBlueThreatsDialog = function (size) {
		openThreatsDialog(size, $scope.blueThreats, $scope.allExternalThreats, 'Blue', function (threats) { $scope.blueThreats = threats; });
	}
	$scope.openInternalThreatsDialog = function (size) {
		openThreatsDialog(size, $scope.internalThreats, $scope.allInternalThreats, 'Internal', function (threats) { $scope.internalThreats = threats; });
	}

	$scope.canCreateGame = function() {
		return $scope.redTrack != null && $scope.whiteTrack != null && $scope.blueTrack != null && $scope.internalTrack != null;
	}

	$scope.getGameArgs = function () {
		if (!$scope.canCreateGame())
			return '';
		var gameArgs = '';

		gameArgs += '-players';
		gameArgs += ' ';
		for (var playerIndex = 0; playerIndex < $scope.players.length; playerIndex++) {
			gameArgs += 'player-index:' + playerIndex;
			gameArgs += ' ';
			gameArgs += 'actions:';
			for (var actionIndex = 0; actionIndex < $scope.players[playerIndex].actions.length; actionIndex++)
				gameArgs += $scope.players[playerIndex].actions[actionIndex].serializationText;
			gameArgs += ' ';
			gameArgs += 'player-color:' + $scope.players[playerIndex].color.model;
			gameArgs += ' ';
		}

		gameArgs += '-external-tracks';
		gameArgs += ' ';
		gameArgs += 'red:' + $scope.redTrack.trackIndex,
		gameArgs += ' ';
		gameArgs += 'white:' + $scope.whiteTrack.trackIndex,
		gameArgs += ' ';
		gameArgs += 'blue:' + $scope.blueTrack.trackIndex,
		gameArgs += ' ';
		gameArgs += '-internal-track';
		gameArgs += ' ';
		gameArgs += $scope.internalTrack.trackIndex,
		gameArgs += ' ';

		gameArgs += '-external-threats';
		gameArgs += ' ';
		if ($scope.redThreats) {
			for (var threatIndex = 0; threatIndex < $scope.redThreats.length; threatIndex++) {
				gameArgs += 'id:' + $scope.redThreats[threatIndex].id;
				gameArgs += ' ';
				gameArgs += 'time:' + $scope.redThreats[threatIndex].timeAppears;
				gameArgs += ' ';
				gameArgs += 'location:red';
				gameArgs += ' ';
			}
		}
		if ($scope.whiteThreats) {
			for (var threatIndex = 0; threatIndex < $scope.whiteThreats.length; threatIndex++) {
				gameArgs += 'id:' + $scope.whiteThreats[threatIndex].id;
				gameArgs += ' ';
				gameArgs += 'time:' + $scope.whiteThreats[threatIndex].timeAppears;
				gameArgs += ' ';
				gameArgs += 'location:white';
				gameArgs += ' ';
			}
		}
		if ($scope.blueThreats) {
			for (var threatIndex = 0; threatIndex < $scope.blueThreats.length; threatIndex++) {
				gameArgs += 'id:' + $scope.blueThreats[threatIndex].id;
				gameArgs += ' ';
				gameArgs += 'time:' + $scope.blueThreats[threatIndex].timeAppears;
				gameArgs += ' ';
				gameArgs += 'location:blue';
				gameArgs += ' ';
			}
		}

		gameArgs += '-internal-threats';
		gameArgs += ' ';
		for (var threatIndex = 0; threatIndex < $scope.internalThreats.length; threatIndex++) {
			gameArgs += 'id:' + $scope.internalThreats[threatIndex].id;
			gameArgs += ' ';
			gameArgs += 'time:' + $scope.internalThreats[threatIndex].timeAppears;
			gameArgs += ' ';
		}

		return gameArgs;
	}
}])
.controller('ActionsModalInstanceCtrl', ['$uibModalInstance', '$scope', 'player', 'allActions', function ($uibModalInstance, $scope, player, allActions) {
	$scope.allActions = allActions;
	$scope.selectedActions = player.actions.slice();
	$scope.playerColor = player.color.model;
	$scope.playerTitle = player.title;
	$scope.cursor = 0;

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

	$scope.newActionHotkey = function (newText) {
		if (newText == undefined) {
			return '';
		}
		var addedAction = false;
		for (var i = 0; i < $scope.allActions.length && !addedAction; i++)
			if ($scope.allActions[i].entryText === newText) {
				$scope.addActionAtCursor($scope.allActions[i]);
				addedAction = true;
			}
		return '';
	};
}])
.controller('TrackModalInstanceCtrl', ['$uibModalInstance', '$scope', 'currentTrack', 'allTracks', 'zone', function ($uibModalInstance, $scope, currentTrack, allTracks, zone) {
	$scope.selectedTrack = currentTrack;
	$scope.allTracks = allTracks;
	$scope.zone = zone;

	$scope.selectTrack = function (track) {
		$scope.selectedTrack = track;
	}

	$scope.ok = function () {
		$uibModalInstance.close($scope.selectedTrack);
	};

	$scope.cancel = function () {
		$uibModalInstance.dismiss('cancel');
	};
}])
.controller('ThreatsModalInstanceCtrl', ['$uibModalInstance', '$scope', 'currentThreats', 'allThreats', 'zone', function ($uibModalInstance, $scope, currentThreats, allThreats, zone) {
	$scope.selectedThreats = currentThreats.slice();
		$scope.allTimes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
	$scope.allThreats = allThreats;
	$scope.zone = zone;
	$scope.selectThreats = function(threats) {
		$scope.selectedThreats = threats;
	}

	$scope.$watch('threatsGroupedByType', function(newValue) {
		$scope.threats = newValue.seriousThreats;
	});
	$scope.$watch('threats', function() {
		$scope.selectedThreatToAdd = null;
		$scope.selectedTimeOfThreatToAdd = null;
	});
	$scope.threatsGroupedByType = allThreats.whiteThreats;

	$scope.selectThreatToAdd = function(threat) {
		$scope.selectedThreatToAdd = threat;
	}

	$scope.addThreat = function () {
		var newThreat = cloneThreat($scope.selectedThreatToAdd);
		newThreat.timeAppears = $scope.selectedTimeOfThreatToAdd;
		$scope.selectedThreats.push(newThreat);
		$scope.selectedThreatToAdd = null;
		$scope.selectedTimeOfThreatToAdd = null;
	}

	$scope.removeThreat = function(threat) {
		$scope.selectedThreats.splice($scope.selectedThreats.indexOf(threat), 1);
	}

	$scope.ok = function () {
		$uibModalInstance.close($scope.selectedThreats);
	};

	$scope.cancel = function () {
		$uibModalInstance.dismiss('cancel');
	};
}]);
