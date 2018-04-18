'use strict';

angular.module('followingList', ['ngRoute'])
  .component('followingList', {
    templateUrl: '/client/app/home/home.html',
    controller: ['$http', function FollowingListController($http) {
      var self = this;

      const requestOptions = {
        headers: {'session': '48449768ab25e7427a5ab49df1237b2c'}
      };

      $http.get('/twitterapi/', requestOptions).then(function (response) {
        self.tweets = response.data;
      });
    }]
});