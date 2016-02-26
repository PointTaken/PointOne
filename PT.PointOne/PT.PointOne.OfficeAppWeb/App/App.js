/* Common app functionality */

var app = (function () {
    "use strict";

    var app = {};

    // Common initialization function (to be called from each page)
    app.initialize = function () {
        $('body').append(
            '<div id="notification-message">' +
                '<div class="padding">' +
                    '<div id="notification-message-close"></div>' +
                    '<div id="notification-message-header"></div>' +
                    '<div id="notification-message-body"></div>' +
                '</div>' +
            '</div>');

        $('#notification-message-close').click(function () {
            $('#notification-message').hide();
        });


        // After initialization, expose a common notification function
        app.showNotification = function (header, text) {
            $('#notification-message-header').text(header);
            $('#notification-message-body').text(text);
            $('#notification-message').slideDown('fast');
        };
    };

    return app;
})();

var myapp = angular.module("myapp", ['ngRoute', 'AdalAngular']);
myapp.config(['$httpProvider', '$routeProvider', 'adalAuthenticationServiceProvider', '$locationProvider', function ($httpProvider, $routeProvider, adalAuthenticationServiceProvider, $locationProvider) {
   
    $routeProvider.
    when('/home', {
        controller:'Ctrl',
        templateUrl:'/App/Home/Home.html',
        requireADLogin:true}).
    otherwise({ redirectTo: '/home' });

      /*  var endpoints = {
        '/App/Home/Home.html': 'resource1'  
    };*/   
              
    adalAuthenticationServiceProvider.init(
        {         
            tenant: 'aspc1606.onmicrosoft.com',
            clientId: '1c166848-e93c-4b24-86f1-7deffbd04321',
         //   loginResource: 'loginResource123',
            redirectUri: 'http://localhost:27976/App/Home/Home.html',
          //  endpoints: endpoints  // optional
        },
        $httpProvider   // pass http provider to inject request interceptor to attach tokens
        );

    $locationProvider.html5Mode(true);
}]);


myapp.controller('Ctrl', ['$http', '$scope', '$interval', 'adalAuthenticationService', function ($http, $scope, $interval, adalAuthenticationService) {
    $scope.BarOpen = true; 

   
   // console.log(adalAuthenticationService.userInfo);
    $scope.Status = adalAuthenticationService.userInfo.isAuthenticated;
    
    $scope.Login = function()
    {
      adalAuthenticationService.login();
    }
    /* News */ 
    var news = [{Text: "Iron man destroyed $533.224.221,32 worth of the city today. Citizens rejoice as the evil Dr. Doom's evil plans once again are flaunted."},
        {Text:"Captain America has gotten a cold and won't be doing any crimefighting today. Violence, madness and destruction must be expected."},
        { Text: "Climate crisis: Natural or caused by evil villain? "},
        { Text:"Mysterious light seen on the sky. Loud noises. Natural phenomenon or Dr. Doom's antics again?"},
        { Text: "Superhero crimefighting posing serious threat to national security, says unbiased security expert."}
    ];
    var idx = 0;
    $scope.LatestNews = news[idx];
  
    $interval(function () {     
        idx++;
        if(idx > news.length-1)
            idx = 0;
        $scope.LatestNews = news[idx];
    }, 5000);
    

    /* Realtime data */
    $scope.BarInfo =
    {
        SoldTonight: 34,
        SoldLastNight: 433,
        PatronsActive: 49,
        MoneyEarnt: 455323,
        MoneySpent: 36694,
        HappyHour: false,
        ServingStopped:false
    };
    $scope.AutoManagementActive = false;
    $scope.UserAuthenticated = false; 
    $scope.ActivateHappyHour = function()
    {
        if(!scope.UserAuthenticated)
            AuthenticateAD();
        $scope.BarInfo.HappyHour = !$scope.BarInfo.HappyHour;
    }

    function AuthenticateAD()
    {

    }
}]);