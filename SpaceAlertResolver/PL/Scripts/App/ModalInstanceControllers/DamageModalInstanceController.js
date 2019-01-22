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
