'use strict';

angular.
  module('twitterApp').
  config(['$locationProvider' ,'$routeProvider',
    function config($locationProvider, $routeProvider) {
      //$locationProvider.hashPrefix('/client/');

      $routeProvider.
        when('/', {
          template: '<following-list></following-list>'
        }).
        when('/tweet/', {
          template: '<tweet-list></tweet-list>'
        }).
        otherwise('/client/');
    }
  ]);