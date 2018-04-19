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
        when('/', {
          template: '<home-list></home-list>'
        }).
        otherwise('/');
    }
  ]);