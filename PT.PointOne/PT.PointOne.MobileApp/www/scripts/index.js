// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );
        var lock = new Auth0Lock(
      // All these properties are set in auth0-variables.js
      AUTH0_CLIENT_ID,
      AUTH0_DOMAIN
    );

        var userProfile;

        $('.btn-login').click(function (e) {
            e.preventDefault();
            lock.show(function (err, profile, token) {
                if (err) {
                    // Error callback
                    console.log("There was an error");
                    alert("There was an error logging in");
                } else {
                    // Success calback

                    // Save the JWT token.
                    localStorage.setItem('userToken', token);

                    // Save the profile
                    userProfile = profile;

                    $('.login-box').hide();
                    $('.logged-in-box').show();
                    $('.nickname').text(profile.nickname);
                    $('.nickname').text(profile.name);
                    $('.avatar').attr('src', profile.picture);
                }
            });
        });

        $.ajaxSetup({
            'beforeSend': function (xhr) {
                if (localStorage.getItem('userToken')) {
                    xhr.setRequestHeader('Authorization',
                          'Bearer ' + localStorage.getItem('userToken'));
                }
            }
        });

        $('.btn-api').click(function (e) {
            // Just call your API here. The header will be sent
            $.ajax({
                url: 'https://pointone.eu.auth0.com',
                method: 'GET'
            }).then(function (data, textStatus, jqXHR) {
                alert("The request to the secured enpoint was successfull");
            }, function () {
                alert("You need to download the server seed and start it to call this API");
            });
        });

    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };
} )();