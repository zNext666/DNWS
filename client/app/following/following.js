'use strict';

angular.module('followingList', ['ngRoute'])
  .component('followingList', {
    templateUrl: 'following/following.html',
    controller: ['$http', '$rootScope', function TweetListController($http, $rootScope) {
      var self = this;

      const requestOptions = {
          headers: { 'X-session': $rootScope.x_session }
      };

      $http.get('http://localhost:8080/twitterapi/following/', requestOptions).then(function (response) {
        self.followings = response.data;
      });

      self.sendFollowing = function sendFollowing(followingname)
      {
        const data = "followingname=" + encodeURIComponent(followingname);
        //console.log("send Following");
        //console.log(data);
        $http.post('http://localhost:8080/twitterapi/following/', data, requestOptions).then(function (response) {
          //self.followings += response.response;
        });
      }

      self.sendUnfollowing = function sendUnfollowing(followingname)
      {
        const data = "followingname=" + encodeURIComponent(followingname);
        //console.log(data);
        $http.delete('http://localhost:8080/twitterapi/following/?'+data,requestOptions).then(function (response){

        });
      }


    }]
});