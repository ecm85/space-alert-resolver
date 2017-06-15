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
