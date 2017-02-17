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
							return $http.get('api/SpaceAlertApi/NewGameInput').then(function (response) { return response.data; });
						}]
					}
				})
				.when('/Manual',
				{
					templateUrl: 'templates/Manual',
					controller: 'ManualController'
				})
				.when('/Resolution',
				{
					templateUrl: 'templates/Resolution',
					controller: 'ResolutionController',
					resolve: {
						'gameData': ['$location', '$http', 'newGameData', function ($location, $http, newGameData) {
							if (newGameData.canCreateGame())
								return $http({
										url: 'api/SpaceAlertApi/ProcessGame',
										method: "POST",
										data: newGameData.manualData || newGameData.getGameArgs(),
										headers: { 'Content-Type': 'application/json' }
									})
									.then(function(response) { return response.data; });
							else
								$location.path('/');
						}]
					}
				})
				.otherwise({
					redirectTo: '/Input'
				});
		}
	]);
