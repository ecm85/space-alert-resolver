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
