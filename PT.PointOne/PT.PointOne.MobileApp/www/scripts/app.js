﻿$("#order").on("click", function() {
    order();
});

$("#pour").on("click", function() {
    PourOrder();
});

yam.getLoginStatus(
  function (response) {
      if (response.authResponse) {
          console.log("logged in");
          yam.platform.request({
              url: "users.json",     //this is one of many REST endpoints that are available
              method: "GET",
              data: {    //use the data object literal to specify parameters, as documented in the REST API section of this developer site
                  "letter": "a",
                  "page": "2",
              },
              success: function (user) { //print message response information to the console
                  alert("The request was successful.");
                  console.dir(user);
              },
              error: function (user) {
                  alert("There was an error with the request.");
              }
          });
      }
      else {
          alert("not logged in");
      }
  }
);

yam.connect.loginButton('#yammer-login', function (resp) {
    if (resp.authResponse) {
        document.getElementById('yammer-login').innerHTML = 'Welcome to Yammer!';
    }
});

function order() {
    var name = $("#name").val();
    var status = $("#status");
    var data = { OrderId: "123", UserId: "31", Price: "49" };
    $.ajax({
        url: "http://pointone.azurewebsites.net/Order/New",
        type: "POST",
        crossDomain: true,
        dataType: "json",
        headers: {
            "Access-Control-Allow-Origin": "*"
        },
        data: data,
        success: function (d) {
            window.sessionStorage.setItem("requestId", d.RequestId);
            sessionStorage["requestId"] = d.RequestId;
            status.html("Ordered, ready to pour!");
            $("#order").fadeOut();
            $("#pour").fadeIn();
        }
    });
}

function PourOrder() {
    var status = $("#status");
    var data = { RequestId: window.sessionStorage.getItem("requestId") };
    $.ajax({
        url: "http://pointone.azurewebsites.net/Order/Pour",
        type: "POST",
        crossDomain: true,
        dataType: "json",
        headers: {
            "Access-Control-Allow-Origin": "*"
        },
        data: data,
        success: function (d) {
            if (d.Status === 1) {
                status.html("Tap is busy right now, try again when it's free!");
                return;
            }
            if (d.Status === 4) {
                status.html("Error: " + d.Message + " Try again later!");
                return;
            }
            status.html('Pouring, please wait!');
            $("#pour").fadeOut();
            GetPourStatus();
        }
    });
}

function GetPourStatus() {
    var interval = setInterval(function () {
        var rid = $("#requestid").val();
        var status = $("#status");
        $.get("/Order/Status/" + rid, function (d) {
            status.html(ParseStatus(d.Status));
            console.log(d);
            if (d.Status == 5) { // Order complete
                clearInterval(interval);
                $("#order").fadeIn();
            }
            if (d.Status == 4) { // Order error, try again
                clearInterval(interval);
                status.html('ERROR: ' + d.Message);
                $("#pour").fadeIn();
            }
        });
    }, 500);
}

function ParseStatus(status) {
    if (status == 0)
        return "WAITING_FOR_PAYMENT";
    if (status == 1)
        return "Your drink is queued";
    if (status == 2)
        return "Drink is ready to be poured... waiting for device to respond to request.";
    if (status == 3)
        return "Your drink is being poured by device!";
    if (status == 4)
        return "An error occured :(";
    if (status == 5)
        return "Enjoy! :) ";
}