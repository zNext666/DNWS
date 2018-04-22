'use strict';

angular.
  module('twitterApp').
  config(['$locationProvider' ,'$routeProvider',
    function config($locationProvider, $routeProvider) {
      $locationProvider.hashPrefix('!');

      $routeProvider.
        when('/tweet/', {
          template: '<tweet-list></tweet-list>'
        }).
        when('/following/', {
          template: '<following-list></following-list>'
        }).
        when('/login/', {
          template: '<login-page></login-page>'
        }).
        when('/', {
          template: '<home-list></home-list>'
        }).
        otherwise('/');
    }
  ])
  .run(function($rootScope, $location, $cookies){
      $rootScope.$on("$routeChangeStart", function(event, next, current) {
        $rootScope.x_session = $cookies.get('x-session');
        if($rootScope.x_session == null) {
         $location.path("/login/");
        }
      });
  }) ;