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
   /*$scope.BarInfo = {
        SoldTonight: 2,
        SoldLastNight: 433,
        PatronsActive: 13,
        MoneyEarnt: 344,
        MoneySpent: 144,
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
    }
   
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
    $scope.CreateChart = function() {
       
            // Run a batch operation against the Excel object model
            Excel.run(function (ctx) {

                // Create a proxy object for the active worksheet
                var sheet = ctx.workbook.worksheets.getActiveWorksheet();

                //Queue commands to set the report title in the worksheet
                sheet.getRange("A1").values = "Sales Report";
                sheet.getRange("A1").format.font.name = "Arial";
                sheet.getRange("A1").format.font.size = 26;
                var sales = 122 + parseInt($scope.BarInfo.SoldTonight);               
                var guests = 122 + parseInt($scope.BarInfo.PatronsActive);
                var earnings = 221 + parseInt($scope.BarInfo.MoneyEarnt);
                var expenses = 23 + parseInt($scope.BarInfo.MoneySpent);
                
                //Create an array containing sample data
                var values = [["What", "Q22015", "Q32015", "Q42015", "Q12016"],
                              ["Total sales", 5000, 7000, 6544, sales],
                              ["Guests", 400, 323, 276, guests],
                              ["Earnings", 12000, 8766, 8456, earnings],
                              ["Expenses", 1550, 1088, 692, expenses]
                      ];

                //Queue a command to write the sample data to the specified range
                //in the worksheet and bold the header row
               var range = sheet.getRange("A2:E6");
               range.values = values;
                sheet.getRange("A2:E6").format.font.bold = true;

                //Queue a command to add a new chart
                var chart = sheet.charts.add("ColumnClustered", range, "auto");

                //Queue commands to set the properties and format the chart
                chart.setPosition("G1", "L10");
                chart.title.text = "Quarterly sales chart";
                chart.legend.position = "right"
                chart.legend.format.fill.setSolidColor("white");
                chart.dataLabels.format.font.size = 15;
                chart.dataLabels.format.font.color = "black";
                var points = chart.series.getItemAt(0).points;
                points.getItemAt(0).format.fill.setSolidColor("pink");
                points.getItemAt(1).format.fill.setSolidColor('indigo');

                //Run the queued-up commands, and return a promise to indicate task completion
                return ctx.sync();
            })
              .then(function () {
                  app.showNotification("Success");
                  console.log("Success!");
              })
            .catch(function (error) {
                // Always be sure to catch any accumulated errors that bubble up from the Excel.run execution
                app.showNotification("Error: " + error);
                console.log("Error: " + error);
                if (error instanceof OfficeExtension.Error) {
                    console.log("Debug info: " + JSON.stringify(error.debugInfo));
                }
            });
        }
    
  
        
        
      function AuthenticateAD()
    {

    }
}]);