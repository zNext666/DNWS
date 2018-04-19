'use strict';

angular.module('homeList', ['ngRoute'])
  .component('homeList', {
    templateUrl: 'home/home.html',
    controller: ['$http', function FollowingListController($http) {
      var self = this;

      const requestOptions = {
        headers: {'X-session': '48449768ab25e7427a5ab49df1237b2c'}
      };

      $http.get('http://localhost:8080/twitterapi/', requestOptions).then(function (response) {
        self.tweets = response.data;
      });
    }]
});