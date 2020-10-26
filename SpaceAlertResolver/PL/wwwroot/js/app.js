angular.module("spaceAlertModule", ['ngAnimate', 'ngSanitize', 'ui.bootstrap', 'cfp.hotkeys', 'ngRoute'])
    .config([
        '$routeProvider',
        function($routeProvider) {
            $routeProvider
                .when('/Input',
                {
                    templateUrl: 'templates/Input',
                    controller: 'InputController',
                    resolve: {
                        'inputData': ['$http', function ($http) {
                            return $http.get('NewGameInput').then(function (response) { return response.data; });
                        }]
                    }
                })
                .when('/Manual',
                {
                    templateUrl: 'templates/Manual',
                    controller: 'ManualController'
                })
                .when('/Error',
                {
                    templateUrl: 'templates/Error'
                })
                .when('/ReportBug',
                {
                    templateUrl: 'templates/ReportBug',
                    controller: 'ReportBugController'
                })
                .when('/Resolution',
                {
                    templateUrl: 'templates/Resolution',
                    controller: 'ResolutionController',
                    resolve: {
                        'gameData': ['$location', '$http', 'newGameData', function ($location, $http, newGameData) {
                            if (newGameData.canCreateGame()) {
                                return $http({
                                        url: 'ProcessGame',
                                        method: "POST",
                                        data: newGameData.manualData || newGameData.getGameArgs(),
                                        headers: { 'Content-Type': 'application/json' }
                                    })
                                    .then(
                                        function(response) {
                                            return response.data;
                                        },
                                        function (response) {
                                            $http({
                                                url: 'SendGameMessage?senderEmailAddress=',
                                                method: "POST",
                                                data: {
                                                    messageText: "Submitted data: " + JSON.stringify(newGameData.manualData || newGameData.getGameArgs())
                                                },
                                                headers: { 'Content-Type': 'application/json' }
                                            });
                                            $location.path('/Error');
                                        });
                            } else {
                                $location.path('/');
                                return null;
                            }
                        }]
                    }
                })
                .otherwise({
                    redirectTo: '/Input'
                });
        }
    ]);

"use strict";

angular.module("spaceAlertModule")
    .controller("InputController",
    [
        "$scope", '$uibModal', 'inputData', 'newGameData', function($scope, $uibModal, inputData, newGameData) {
            $scope.allTracks = inputData.tracks;
            $scope.newGameData = newGameData;
            $scope.playerSpecializations = inputData.playerSpecializations;

            $scope.$watch('newGameData.selectedTracks.redTrack',
                function(newValue) {
                    newGameData.updateAllSelectedTracks();
                    if (!newValue)
                        return;
                    newGameData.checkDuplicateWhiteTrack(newValue);
                    newGameData.checkDuplicateBlueTrack(newValue);
                    newGameData.checkDuplicateInternalTrack(newValue);
                });
            $scope.$watch('newGameData.selectedTracks.whiteTrack',
                function(newValue) {
                    newGameData.updateAllSelectedTracks();
                    if (!newValue)
                        return;
                    newGameData.checkDuplicateRedTrack(newValue);
                    newGameData.checkDuplicateBlueTrack(newValue);
                    newGameData.checkDuplicateInternalTrack(newValue);
                });
            $scope.$watch('newGameData.selectedTracks.blueTrack',
                function(newValue) {
                    newGameData.updateAllSelectedTracks();
                    if (!newValue)
                        return;
                    newGameData.checkDuplicateRedTrack(newValue);
                    newGameData.checkDuplicateWhiteTrack(newValue);
                    newGameData.checkDuplicateInternalTrack(newValue);
                });
            $scope.$watch('newGameData.selectedTracks.internalTrack',
                function(newValue) {
                    newGameData.updateAllSelectedTracks();
                    if (!newValue)
                        return;
                    newGameData.checkDuplicateRedTrack(newValue);
                    newGameData.checkDuplicateWhiteTrack(newValue);
                    newGameData.checkDuplicateBlueTrack(newValue);
                });

            $scope.allInternalThreats = inputData.allInternalThreats;
            $scope.allExternalThreats = inputData.allExternalThreats;

            $scope.$watchCollection('newGameData.selectedThreats.redThreats',
                function() {
                    newGameData.updateAllSelectedExternalThreats();
                    newGameData.updateAllSelectedThreats();
                });
            $scope.$watchCollection('newGameData.selectedThreats.whiteThreats',
                function() {
                    newGameData.updateAllSelectedExternalThreats();
                    newGameData.updateAllSelectedThreats();
                });
            $scope.$watchCollection('newGameData.selectedThreats.blueThreats',
                function() {
                    newGameData.updateAllSelectedExternalThreats();
                    newGameData.updateAllSelectedThreats();
                });
            $scope.$watchCollection('newGameData.selectedThreats.internalThreats',
                function () {
                    newGameData.updateAllSelectedThreats();
                });

            $scope.allSingleActions = inputData.singleActions;
            $scope.allDoubleActions = inputData.doubleActions;
            $scope.specializationActions = inputData.specializationActions;

            $scope.$watch('newGameData.selectedPlayerCountRadio.model',
                function(newPlayerCount) {
                    newGameData.selectPlayerCount(newPlayerCount);
                });
            newGameData.players.forEach(function(player, index) {
                $scope.$watch(
                    function() {
                        return newGameData.players[index].color.model;
                    },
                    function(newValue, oldValue) {
                        newGameData.players.forEach(function (player, exisitingPlayerIndex) {
                            if (index !== exisitingPlayerIndex && player.color.model === newValue)
                                player.color.model = oldValue;
                        });
                    });
            });

            $scope.allDamageTokens = inputData.allDamageTokens;
            $scope.damageableZones = inputData.damageableZones;

            $scope.damageableZones.forEach(function (damageableZone) {
                if (newGameData.damage[damageableZone] == null)
                    newGameData.damage[damageableZone] = {};
            });

            var setCalledInThreat = function (bonusThreatSetterFn, allThreats, allUsedThreats) {
                var modal = $uibModal.open({
                    animation: true,
                    templateUrl: 'templates/threatsModal',
                    controller: 'ThreatsModalInstanceCtrl',
                    size: 'lg',
                    resolve: {
                        allThreats: function () {
                            return allThreats;
                        },
                        allUsedThreats: function () {
                            return allUsedThreats;
                        },
                        zone: function () {
                            return '';
                        },
                        threatAppearsNormally: function () {
                            return false;
                        }
                    }
                });
                modal.result.then(bonusThreatSetterFn);
            }
            
            $scope.setCalledInExternalThreat = function (threat) {
                setCalledInThreat(function(newThreat) {
                    threat.bonusExternalThreat = newThreat;
                }, $scope.allExternalThreats, $scope.newGameData.allSelectedExternalThreats);
            }

            $scope.setCalledInInternalThreat = function (threat) {
                setCalledInThreat(function (newThreat) {
                    threat.bonusInternalThreat = newThreat;
                }, $scope.allInternalThreats, $scope.newGameData.allSelectedInternalThreats);
            }

            $scope.clearBonusThreat = function(threat) {
                threat.bonusExternalThreat = null;
                threat.bonusInternalThreat = null;
            }

            $scope.anyThreatsCallInBonusThreats = function() {
                return _.some(newGameData.allSelectedThreats, function (threat) {
                    return threat.needsBonusExternalThreat || threat.needsBonusInternalThreat;
                });
            }

            $scope.animationsEnabled = true;

            $scope.openActionsDialog = function(player, size) {
                $uibModal.open({
                    animation: true,
                    templateUrl: 'templates/actionsModal',
                    controller: 'ActionsModalInstanceCtrl',
                    size: size,
                    resolve: {
                        player: function() {
                            return player;
                        },
                        allSingleActions: function () {
                            return $scope.allSingleActions;
                        },
                        allDoubleActions: function () {
                            return $scope.allDoubleActions;
                        },
                        useDoubleActions: function() {
                            return newGameData.useDoubleActions;
                        },
                        playerSpecializationActions: function() {
                            return $scope.specializationActions.filter(function(specializationAction) {
                                return specializationAction.playerSpecialization === player.playerSpecialization;
                            });
                        },
                        useSpecializations: function () {
                            return newGameData.useSpecializations && player.playerSpecialization != null;
                        }
                    }
                });
            };
        }
    ]);

"use strict";

angular.module("spaceAlertModule")
    .controller("ManualController",
    [
        "$scope", 'newGameData', function($scope, newGameData) {
            $scope.newGameData = newGameData;
        }
    ]);

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

"use strict";

angular.module("spaceAlertModule")
    .controller("ResolutionController",
    [
        "$scope", "gameData", '$interval', '$animate', '$uibModal', function($scope, gameData, $interval, $animate, $uibModal) {
            //TODO: Remove the need for this (shouldn't need to do this check, on error it sohuld never hit this controller)
            //Check if gamedata got populated - will be undefined if there was an error
            if (!gameData)
                return;

            $animate.enabled(false);
            $scope.gameData = gameData;
            var turnCount = $scope.gameData.length;
            $scope.playing = null;
            $scope.turnsPerTwoSeconds = 5;
            let currentPhaseIndex = 0;
            let currentTurnIndex = 0;
            let currentSubPhaseIndex = 0;

            var stop = function () {
                if ($scope.playing != null) {
                    $interval.cancel($scope.playing);
                    $scope.playing = null;
                }
            };

            var selectSubPhase = function(newSubPhaseIndex) {
                currentSubPhaseIndex = newSubPhaseIndex;
            }
            var selectPhase = function (newPhaseIndex) {
                currentPhaseIndex = newPhaseIndex;
                selectSubPhase(0);
            }
            var selectTurn = function(newTurnIndex) {
                currentTurnIndex = newTurnIndex;
                selectPhase(0);
            }

            var play = function() {
                $scope.playing = $interval(function() {
                        if (!$scope.isAtLastSubPhase())
                            selectSubPhase(currentSubPhaseIndex + 1);
                        else if (!$scope.isAtLastPhase())
                            selectPhase(currentPhaseIndex + 1);
                        else if (!$scope.isAtLastTurn())
                            selectTurn(currentTurnIndex + 1);
                        else {
                            stop();
                        }
                    },
                    2000 / $scope.turnsPerTwoSeconds);
            };
            $scope.$watch('turnsPerTwoSeconds',
                function() {
                    if ($scope.playing != null) {
                        stop();
                        play();
                    }
                });

            $scope.isAtFirstSubPhase = function() {
                return currentSubPhaseIndex === 0;
            }
            $scope.isAtFirstPhase = function() {
	            return currentPhaseIndex === 0;
            }
            $scope.isAtFirstTurn = function() {
                return currentTurnIndex === 0;
            }
            $scope.isAtLastSubPhase = function() {
                return currentSubPhaseIndex === $scope.getCurrentPhase().subPhases.length - 1;
            }
            $scope.isAtLastPhase = function() {
                return currentPhaseIndex === $scope.getCurrentTurn().phases.length - 1;
            }
            $scope.isAtLastTurn = function() {
                return currentTurnIndex === turnCount - 1;
            }

            $scope.getCurrentTurn = function() {
                return $scope.gameData[currentTurnIndex];
            }

            $scope.getCurrentPhase = function() {
                return $scope.getCurrentTurn().phases[currentPhaseIndex];
            }

            $scope.getCurrentSubPhase = function() {
                return $scope.getCurrentPhase().subPhases[currentSubPhaseIndex];
            }

            $scope.goBackOneSubPhase = function () {
                stop();
                if (!$scope.isAtFirstSubPhase())
                    selectSubPhase(currentSubPhaseIndex - 1);
                else if (!$scope.isAtFirstPhase()) {
                    selectPhase(currentPhaseIndex - 1);
                    selectSubPhase($scope.getCurrentPhase().subPhases.length - 1);
                }
                else if (!$scope.isAtFirstTurn()) {
                    selectTurn(currentTurnIndex - 1);
                    selectPhase($scope.getCurrentTurn().phases.length - 1);
                    selectSubPhase($scope.getCurrentPhase().subPhases.length - 1);
                }
            }
            $scope.goForwardOneSubPhase = function () {
                stop();
                if (!$scope.isAtLastSubPhase())
                    selectSubPhase(currentSubPhaseIndex + 1);
                else if (!$scope.isAtLastPhase()) {
                    selectPhase(currentPhaseIndex + 1);
                }
                else if (!$scope.isAtLastTurn()) {
                    selectTurn(currentTurnIndex + 1);
                }
            }

            $scope.goBackOnePhase = function () {
                stop();
                if (!$scope.isAtFirstPhase())
                    selectPhase(currentPhaseIndex - 1);
                else if (!$scope.isAtFirstTurn()) {
                    selectTurn(currentTurnIndex - 1);
                    selectPhase($scope.getCurrentTurn().phases.length - 1);
                }
            	//TODO: Add else to go to beginning of game (will result in changing subphases)?
            }
            $scope.goForwardOnePhase = function() {
                stop();
                if (!$scope.isAtLastPhase())
                    selectPhase(currentPhaseIndex + 1);
                else if (!$scope.isAtLastTurn()) {
                    selectTurn(currentTurnIndex + 1);
                }
            	//TODO: Add else to go to end of game (will result in changing subphases)?
            }

            $scope.goBackOneTurn = function () {
                stop();
                if (!$scope.isAtFirstTurn()) {
                    selectTurn(currentTurnIndex - 1);
                }
				//TODO: Add else to go to beginning of game (will result in changing phase and subphase)?
            }
            $scope.goForwardOneTurn = function () {
                stop();
                if (!$scope.isAtLastTurn()) {
                    selectTurn(currentTurnIndex + 1);
                }
            	//TODO: Add else to go to end of game (will result in changing phase and subphase)?
            }

            $scope.playPause = function() {
                if ($scope.playing)
                    stop();
                else
                    play();
            }
            $scope.goToStart = function() {
                stop();
                selectTurn(0);
            }
            $scope.goToEnd = function() {
                stop();
                selectTurn(turnCount - 1);
                selectPhase($scope.getCurrentTurn().phases.length - 1);
                selectSubPhase($scope.getCurrentPhase().subPhases.length - 1);
            }
            $scope.$on('$destroy',
                function() {
                    stop();
                });

            $scope.getActionCursor = function() {
                return currentTurnIndex;
            }

            $scope.openDamageDialog = function (size) {
                stop();
                $uibModal.open({
                    animation: true,
                    templateUrl: 'templates/damageModal',
                    controller: 'DamageModalInstanceCtrl',
                    size: size,
                    resolve: {
                      
                        redDamage: function () {
                            return $scope.getCurrentSubPhase().redZone.unusedDamage;
                        },
                        blueDamage: function () {
                            return $scope.getCurrentSubPhase().blueZone.unusedDamage;
                        },
                        whiteDamage: function () {
                            return $scope.getCurrentSubPhase().whiteZone.unusedDamage;
                        }
                    }
                });
            };

            selectTurn(0);
        }
    ]);

"use strict";

angular.module("spaceAlertModule")
    .directive('bonusThreatEntry',
        function () {
            return {
                templateUrl: 'templates/bonusThreatEntry',
                restrict: 'E',
                scope: {
                    threat: '=',
                    bonusThreat: '=',
                    setBonusThreat: '&',
                    clearBonusThreat: '&'
                },
                controller: [
                    '$scope',
                    function BonusThreatEntryController($scope) {
                        $scope.removeThreat = function() {
                            $scope.clearBonusThreat();
                        }
                    }
                ]
            };
        });


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


angular.module("spaceAlertModule")
.directive('routeLoadingIndicator', ['$rootScope', function($rootScope) {
        return {
            restrict: 'E',
            templateUrl: 'templates/loading',
            link: function (scope, elem, attrs) {
                scope.isRouteLoading = false;

                $rootScope.$on('$routeChangeStart', function () {
                    scope.isRouteLoading = true;
                });

                $rootScope.$on('$routeChangeSuccess', function () {
                    scope.isRouteLoading = false;
                });
            }
        };
    }]);

"use strict";

angular.module("spaceAlertModule")
    .directive('standardZone',
        function() {
            return {
                templateUrl: 'templates/standardZone',
                restrict: 'E',
                scope: {
                    zone: '=',
                    zoneId: '='
                }
            }
        });

"use strict";

angular.module("spaceAlertModule")
    .directive('threat',
        function() {
            return {
                templateUrl: 'templates/threat',
                restrict: 'E',
                scope: {
                    threatIndex: '=',
                    threat: '=',
                    trackId: '=',
                    removable: '=',
                    removeThreat: '&?'
                }
            };
        });

"use strict";

angular.module("spaceAlertModule")
    .directive('track',
        function() {
            return {
                templateUrl: 'templates/track',
                restrict: 'E',
                scope: {
                    track: '=',
                    trackId: '='
                }
            };
        });

"use strict";

angular.module("spaceAlertModule")
    .directive('trackWithThreats',
        function() {
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
                controller: [
                    '$scope', '$uibModal', function TrackWithThreatsController($scope, $uibModal) {
                        $scope.getThreatCornerX = function(index) {
                            var threatElement = $('#threat' + $scope.trackId + index);
                            if (threatElement.offset())
                                return threatElement.offset().left;
                            return 0;
                        }
                        $scope.getThreatCornerY = function(index) {
                            var threatElement = $('#threat' + $scope.trackId + index);
                            if (threatElement.offset())
                                return threatElement.offset().top;
                            return 0;
                        }
                        $scope.getTrackSpaceCornerX = function(threat) {
                            var spaceElement = $('#space' + $scope.trackId + threat.position);
                            if (spaceElement.offset())
                                return spaceElement.offset().left + spaceElement.outerWidth() - 1;
                            return 0;
                        }
                        $scope.getTrackSpaceCornerY = function(threat) {
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
                        $scope.getStationCornerY = function(threat, station) {
                            var stationElement = $('#' + station.toLowerCase() + 'threats');
                            if (stationElement.offset())
                                return stationElement.offset().top;
                            return 0;
                        }

                        $scope.configureTrack = function() {
                            if (!$scope.trackIsConfigurable)
                                return;
                            var modal = $uibModal.open({
                                animation: true,
                                templateUrl: 'templates/trackModal',
                                controller: 'TrackModalInstanceCtrl',
                                size: 'lg',
                                resolve: {
                                    currentTrack: function() {
                                        return $scope.track;
                                    },
                                    allTracks: function() {
                                        return $scope.allTracks;
                                    },
                                    zone: function() {
                                        return $scope.zoneDescription;
                                    },
                                    usedTracks: function() {
                                        return $scope.allUsedTracks;
                                    }
                                }
                            });
                            modal.result.then(function(selectedTrack) {
                                $scope.track = selectedTrack;
                            });
                        }

                        $scope.addNewThreat = function() {
                            if (!$scope.threatsAreConfigurable)
                                return;
                            var modal = $uibModal.open({
                                animation: true,
                                templateUrl: 'templates/threatsModal',
                                controller: 'ThreatsModalInstanceCtrl',
                                size: 'lg',
                                resolve: {
                                    allThreats: function() {
                                        return $scope.allThreats;
                                    },
                                    allUsedThreats: function() {
                                        return $scope.allUsedThreats;
                                    },
                                    zone: function() {
                                        return $scope.zoneDescription;
                                    },
                                    threatAppearsNormally: function() {
                                        return true;
                                    }
                                }
                            });
                            modal.result.then(function(threat) {
                                $scope.threats.push(threat);
                            });
                        }

                        $scope.showThreats = function() {
                            return $scope.threats || $scope.threatsAreConfigurable;
                        }

                        $scope.canAddNewThreat = function() {
                            return $scope.threatsAreConfigurable && $scope.threats.length < 3;
                        }

                        $scope.removeThreat = function(threatToRemove) {
                            _.remove($scope.threats, function(threat) { return threat.id === threatToRemove.id });
                        }
                    }
                ]
            }
        });

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

"use strict";

angular.module("spaceAlertModule")
    .controller('DamageModalInstanceCtrl',
    [
        '$uibModalInstance', '$scope', 'redDamage', 'whiteDamage', 'blueDamage',
        function ($uibModalInstance, $scope, redDamage, whiteDamage, blueDamage) {
            $scope.damageableZones = [
                {
                    name: "Red",
                    damage: redDamage
                },
                {
                    name: "Blue",
                    damage: blueDamage
                },
                {
                    name: "White",
                    damage: whiteDamage
                },
            ];

            $scope.ok = function() {
                $uibModalInstance.close();
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        }
    ]);

"use strict";

var cloneThreat = function(threat) {
    return {
        threatType: threat.threatType,
        threatDifficulty: threat.threatDifficulty,
        position: threat.position,
        remainingHealth: threat.remainingHealth,
        speed: threat.speed,
        fileName: threat.fileName,
        timeAppears: threat.timeAppears,
        id: threat.id,
        displayName: threat.displayName,
        points: threat.points,
        buffCount: threat.buffCount,
        debuffCount: threat.debuffCount,
        needsBonusExternalThreat: threat.needsBonusExternalThreat,
        needsBonusInternalThreat: threat.needsBonusInternalThreat,
        bonusInternalThreat: threat.bonusInternalThreat,
        bonusExternalThreat: threat.bonusExternalThreat,

        shields: threat.shields,
        currentZone: threat.currentZone,

        totalInaccessibility: threat.totalInaccessibility,
        displayOnTrackStations: threat.displayOnTrackStations
    };
};

angular.module("spaceAlertModule")
    .controller('ThreatsModalInstanceCtrl',
    [
        '$uibModalInstance', '$scope', 'allThreats', 'allUsedThreats', 'zone', 'threatAppearsNormally',
        function ($uibModalInstance, $scope, allThreats, allUsedThreats, zone, threatAppearsNormally) {
            $scope.threatAppearsNormally = threatAppearsNormally;
            $scope.allTimes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
            $scope.allThreats = allThreats;
            $scope.zone = zone;
            $scope.allUsedThreats = allUsedThreats;

            $scope.$watch('threatsGroupedByType',
                function(newValue) {
                    $scope.threatsToChooseFrom = newValue.minorThreats;
                });
            $scope.$watch('threatsToChooseFrom',
                function() {
                    $scope.selectedThreatToAdd = null;
                    $scope.selectedTimeOfThreatToAdd = null;
                });
            $scope.threatsGroupedByType = allThreats.whiteThreats;

            $scope.selectThreatToAdd = function(threat) {
                $scope.selectedThreatToAdd = threat;
            }

            $scope.getAvailableThreats = function() {
                return _.differenceBy($scope.threatsToChooseFrom, $scope.allUsedThreats, 'id');
            }

            $scope.ok = function() {
                var newThreat = cloneThreat($scope.selectedThreatToAdd);
                newThreat.timeAppears = $scope.selectedTimeOfThreatToAdd;
                $uibModalInstance.close(newThreat);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        }
    ]);

"use strict";

angular.module("spaceAlertModule")
    .controller('TrackModalInstanceCtrl',
    [
        '$uibModalInstance', '$scope', 'currentTrack', 'allTracks', 'zone', 'usedTracks',
        function($uibModalInstance, $scope, currentTrack, allTracks, zone, usedTracks) {
            $scope.selectedTrack = currentTrack;
            $scope.allTracks = allTracks;
            $scope.zone = zone;
            $scope.usedTracks = usedTracks;

            $scope.trackIsInUse = function(track) {
                if ($scope.selectedTrack === track)
                    return false;
                for (var i = 0; i < $scope.usedTracks.length; i++)
                    if ($scope.usedTracks[i] === track)
                        return true;
                return false;
            }

            $scope.selectTrack = function(track) {
                $scope.selectedTrack = track;
                $uibModalInstance.close($scope.selectedTrack);
            }

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        }
    ]);

var cloneAction = function (action) {
    return {
        hotkey: action.hotkey,
        displayText: action.displayText,
        description: action.description,
        firstAction: action.firstAction,
        secondAction: action.secondAction
    };
};

angular.module("spaceAlertModule")
    .factory('newGameData',
        function() {
            var newGameData = {};
            newGameData.initialize = function() {
                newGameData.selectedTracks = {
                    redTrack: null,
                    whiteTrack: null,
                    blueTrack: null,
                    internalTrack: null
                };
                newGameData.selectedThreats = {
                    redThreats: [],
                    whiteThreats: [],
                    blueThreats: [],
                    internalThreats: []
                };
                newGameData.damage = {};
                newGameData.updateAllSelectedTracks = function() {
                    newGameData.allSelectedTracks = [
                        newGameData.selectedTracks.redTrack,
                        newGameData.selectedTracks.whiteTrack,
                        newGameData.selectedTracks.blueTrack,
                        newGameData.selectedTracks.internalTrack
                    ];
                };
                newGameData.updateAllSelectedTracks();

                newGameData.checkDuplicateRedTrack = function(track) {
                    if (newGameData.selectedTracks.redTrack === track)
                        newGameData.selectedTracks.redTrack = null;
                };
                newGameData.checkDuplicateWhiteTrack = function(track) {
                    if (newGameData.selectedTracks.whiteTrack === track)
                        newGameData.selectedTracks.whiteTrack = null;
                };
                newGameData.checkDuplicateBlueTrack = function(track) {
                    if (newGameData.selectedTracks.blueTrack === track)
                        newGameData.selectedTracks.blueTrack = null;
                };
                newGameData.checkDuplicateInternalTrack = function(track) {
                    if (newGameData.selectedTracks.internalTrack === track)
                        newGameData.selectedTracks.internalTrack = null;
                };

                newGameData.updateAllSelectedExternalThreats = function() {
                    newGameData.allSelectedExternalThreats = []
                        .concat(newGameData.selectedThreats.redThreats)
                        .concat(newGameData.selectedThreats.whiteThreats)
                        .concat(newGameData.selectedThreats.blueThreats);
                };

                newGameData.updateAllSelectedThreats = function () {
                    newGameData.allSelectedThreats = []
                        .concat(newGameData.selectedThreats.redThreats)
                        .concat(newGameData.selectedThreats.whiteThreats)
                        .concat(newGameData.selectedThreats.blueThreats)
                        .concat(newGameData.selectedThreats.internalThreats);
                };

                newGameData.colors = ['blue', 'green', 'red', 'yellow', 'purple'];
                newGameData.playerCounts = [1, 2, 3, 4, 5];

                newGameData.players = [
                    { title: 'Captain', color: { model: newGameData.colors[0] }, actions: _.map(_.range(12), function() { return {}; }) },
                    { title: 'Player 2', color: { model: newGameData.colors[1] }, actions: _.map(_.range(12), function () { return {}; }) },
                    { title: 'Player 3', color: { model: newGameData.colors[2] }, actions: _.map(_.range(12), function () { return {}; }) },
                    { title: 'Player 4', color: { model: newGameData.colors[3] }, actions: _.map(_.range(12), function () { return {}; }) },
                    { title: 'Player 5', color: { model: newGameData.colors[4] }, actions: _.map(_.range(12), function () { return {}; }) }
                ];

                newGameData.selectPlayerCount = function(newPlayerCount) {
                    newGameData.selectedPlayerCountRadio = { model: newPlayerCount };
                    newGameData.players.forEach(function(player, index) {
                        player.isInGame = index < newPlayerCount;
                    });
                };
                newGameData.selectPlayerCount(4);

                newGameData.initializeFromGameArgs = function (args) {
                    var parsed = JSON.parse(args);
                    newGameData.selectedThreats.redThreats = parsed.redThreats;
                    newGameData.selectedThreats.whiteThreats = parsed.whiteThreats;
                    newGameData.selectedThreats.blueThreats = parsed.blueThreats;
                    newGameData.selectedThreats.internalThreats = parsed.internalThreats;
                    newGameData.selectedTracks.redTrack = parsed.redTrack;
                    newGameData.selectedTracks.whiteTrack = parsed.whiteTrack;
                    newGameData.selectedTracks.blueTrack = parsed.blueTrack;
                    newGameData.selectedTracks.internalTrack = parsed.internalTrack;
                    for (var i = 0; i < newGameData.players.length; i++) {
                        if (i < parsed.players.length) {
                            var player = newGameData.players[i];
                            var parsedPlayer = parsed.players[i];
                            player.isInGame = true;
                            player.color.model = newGameData.colors[parsedPlayer.playerColor];
                            player.actions = parsedPlayer.actions;
                            player.playerSpecialization = parsedPlayer.playerSpecialization;
                        }
                    }
                }

                newGameData.getGameArgs = function () {
                    if (!newGameData.canCreateGame())
                        return '';
                    var playersInGame = _.filter(newGameData.players, { isInGame: true });
                    var players = _.map(playersInGame,
                        function (player, index) {
                            return {
                                actions: player.actions,
                                index: index,
                                playerColor: _.findIndex(newGameData.colors, function (color) { return color === player.color.model; }),
                                playerSpecialization: player.playerSpecialization
                            }
                        });
                    var damageModels = [];
                    for (var zoneKey in newGameData.damage) {
                        if (newGameData.damage.hasOwnProperty(zoneKey)) {
                            for (var damageTypeKey in newGameData.damage[zoneKey]) {
                                if (newGameData.damage[zoneKey].hasOwnProperty(damageTypeKey)) {
                                    var damageTokenIsSelected = newGameData.damage[zoneKey][damageTypeKey];
                                    if (damageTokenIsSelected)
                                        damageModels.push({
                                            zoneLocation: zoneKey,
                                            damageToken: damageTypeKey
                                        });
                                }
                            }
                        }
                    }
                    var game = {
                        players: players,
                        redThreats: newGameData.selectedThreats.redThreats,
                        whiteThreats: newGameData.selectedThreats.whiteThreats,
                        blueThreats: newGameData.selectedThreats.blueThreats,
                        internalThreats: newGameData.selectedThreats.internalThreats,
                        redTrack: newGameData.selectedTracks.redTrack,
                        whiteTrack: newGameData.selectedTracks.whiteTrack,
                        blueTrack: newGameData.selectedTracks.blueTrack,
                        internalTrack: newGameData.selectedTracks.internalTrack,
                        initialDamageModels: damageModels
                    };
                    return game;
                }

                newGameData.isMissingBonusThreats = function() {
                    return _.some(newGameData.allSelectedThreats, function(threat) {
                        return (threat.needsBonusExternalThreat && threat.bonusExternalThreat == null) ||
                        (threat.needsBonusInternalThreat && threat.bonusInternalThreat == null);
                    });
                }

                newGameData.canCreateGame = function () {
                    return newGameData.manualData || (!newGameData.isMissingBonusThreats() &&
                        newGameData.selectedTracks.redTrack != null &&
                        newGameData.selectedTracks.whiteTrack != null &&
                        newGameData.selectedTracks.blueTrack != null &&
                        newGameData.selectedTracks.internalTrack != null);
                }

                newGameData.setManualData = function (manualData) {
                    newGameData.manualData = manualData;
                }
            }

            newGameData.initialize();

            return newGameData;
        });
