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
