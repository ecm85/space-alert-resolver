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
