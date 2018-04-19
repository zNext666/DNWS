'use strict';

angular.module('tweetList', ['ngRoute'])
  .component('tweetList', {
    templateUrl: 'tweet/tweet.html',
    controller: ['$http', function TweetListController($http) {
      var self = this;

      const requestOptions = {
        headers: {'X-session': '48449768ab25e7427a5ab49df1237b2c'}
      };

      $http.get('http://localhost:8080/twitterapi/tweet/', requestOptions).then(function (response) {
        self.tweets = response.data;
      });
    }]
});