/* Common app functionality */

var app = (function () {
    var app = {};
    /* P: cGUPArQzOeTGFMzn5QjECaS*/
    
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
myapp.config(['$httpProvider', '$routeProvider', '$locationProvider','adalAuthenticationServiceProvider', function ($httpProvider, $routeProvider, $locationProvider, adalProvider) {
      
    $locationProvider.html5Mode(true);
    adalProvider.init({
        // Use this value for the public instance of Azure AD
        instance: 'https://login.microsoftonline.com/', 
        // The 'common' endpoint is used for multi-tenant applications like this one
        tenant: 'common',
        // Your application id from the registration portal
        clientId: '33a3911c-b3fd-4db2-a59a-61aec072be40',
        // If you're using IE, uncommment this line - the default HTML5 sessionStorage does not work for localhost.
        //cacheLocation: 'localStorage',      
       
    }, $httpProvider);
  
}]);

var k = null;
myapp.controller('Ctrl', ['$http', '$scope', '$interval', 'adalAuthenticationService',
                    function ($http, $scope, $interval, adalService) {
    $scope.BarOpen = true; 
   
    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
    $scope.AccessToken = "";
  // $scope.name = userInfo.profile.
    $scope.loggedIn = function () {
        return adalService.userInfo.isAuthenticated; 
    }

    var userInfo = adalService.userInfo;
    k = userInfo; 
    console.log(userInfo);
                       
    $scope.userInfo = adalService.userInfo;
    $scope.name = ""; 
    if ($scope.userInfo != null && $scope.userInfo.profile != null) {
        $scope.name = $scope.userInfo.profile.name;
    }
    $scope.Login = function()
    {
        //document.location.href =
          "https://login.microsoftonline.com/aspc1606.onmicrosoft.com/oauth2/v2.0/authorize?response_type=code&client_id=33a3911c-b3fd-4db2-a59a-61aec072be40&scope=openid&redirect_uri=http%3A%2F%2Flocalhost%3A27976%2FApp%2FHome%2FHome.html&state=85fe56c5-2122-43ed-889a-c4de303f35e0&client-request-id=75805bdf-1f99-4fa7-9d2d-d6c22feeafa2&x-client-SKU=Js&x-client-Ver=2.0.0-experimental&nonce=ffc91b43-ad74-4281-9ec2-641cab1b9912";
         // "https://login.microsoftonline.com/aspc1606.onmicrosoft.com/oauth2/v2.0/authorize?response_type=id_token&client_id=33a3911c-b3fd-4db2-a59a-61aec072be40&scope=openid%20https%3A%2F%2Fgraph.microsoft.com%2Fmail.send&redirect_uri=http%3A%2F%2Flocalhost%3A27976%2FApp%2FHome%2FHome.html&state=0e4e3cab-bd9c-4bd6-aeeb-e7eca37ec0d2&client-request-id=1490ae5b-91a9-47a4-b4bb-317ebb595393&x-client-SKU=Js&x-client-Ver=2.0.0-experimental&nonce=c12f1fbe-b2bf-4cfb-8d8d-882264475b86";
        adalService.login();
    }

    $scope.Testfunc = function()
    {
       console.log( $http.get('https://graph.microsoft.com/v1.0/me/tasks'));
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
    $http.get("https://pointone.azurewebsites.net/Order/BarStatus").success(function(d) { 
        $scope.BarInfo = d; 
    })
  /*  {
        SoldTonight: 34,
        SoldLastNight: 433,
        PatronsActive: 49,
        MoneyEarnt: 455323,
        MoneySpent: 36694,
        HappyHour: false,
        ServingStopped:false
    };*/
    $scope.AutoManagementActive = false;
    $scope.UserAuthenticated = false; 
    $scope.ActivateHappyHour = function()
    {
        if(!scope.UserAuthenticated)
            AuthenticateAD();
        $scope.BarInfo.HappyHour = !$scope.BarInfo.HappyHour;
    }

    var chat = $.connection.chat;
    console.log(chat);
    chat.name = "Test";
    chat.client.hello = function (message) {     
      
           // console.log("TEST" + message);
            $scope.BarInfo = message;
        
      
    };

    $.connection.hub.start().done(function () {
        //console.log("TEST");
    });

    $scope.Send = function()
    {
        $.connection.hub.start().done(function () {
            console.log("Sending hello");
            chat.server.hello("TEST");
        });
    }
    $(document).ready(function () {
        $("#send").click(function () {

            $.connection.hub.start().done(function () {
                console.log("Sending hello");
                chat.server.hello("TEST");
            });
        });
    });
    $scope.AddToDocument = function () {
        Office.context.document.setSelectedDataAsync(
            [["Beers served", $scope.BarInfo.SoldTonight],
             ["Patrons", $scope.BarInfo.PatronsActive],
             ["Money earned", $scope.BarInfo.MoneyEarnt],
            ["Money spent", $scope.BarInfo.MoneySpent]
            ], { coercionType: Office.CoercionType.Matrix }
        ,function (asyncResult) {
            if (asyncResult.status == Office.AsyncResultStatus.Failed) {
                console.log(asyncResult.error.message);
            }
        });
    }
        
        
      function AuthenticateAD()
    {

    }
}]);