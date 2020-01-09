// *******************************************************************************************************
// *******************************************************************************************************
// *******************************************************************************************************

// PLEASE SET THE FOLLOWING MISSING INFORMATION.

// WebApi Server which will create the new account and return the new token.
var webApiUrl = "http://localhost:54077/";
var authenticateRoute = "Authenticate";
var testAuthenticationRoute = "AuthenticateTest";

// AUTH0 CREDENTIALS.
var auth0ClientId = "Place Your Auth0 ClientId in here";
var auth0Domain = "<place your auth0 tenant here>.au.auth0.com";


// *******************************************************************************************************
// *******************************************************************************************************
// *******************************************************************************************************




var lock;
var idTokenLocalStorageKey = "idToken";

function initializeAuth() {
    
    // Initializing our Auth0Lock
    var options = {
        auth: {
            responseType: "token id_token",
            params: { scope: "openid profile email" }
        }
    };

    lock = new Auth0Lock(
        auth0ClientId,
        auth0Domain,
        options
    );

    // Listening for the authenticated event
    lock.on("authenticated", function (authResult) {

        authenticateWithServer(authResult.idToken);
    });
}

function showSignInDialog() {
    lock.show();
}

function signOut() {
    removeIdToken();
    lock.logout({
        returnTo: "http://localhost:49497"
    });
}

function getIdToken() {
    return localStorage.getItem(idTokenLocalStorageKey);
}

function setIdToken() {
    localStorage.setItem(idTokenLocalStorageKey);
}

function removeIdToken() {
    localStorage.removeItem(idTokenLocalStorageKey);
}

function authenticateWithServer(idToken) {

    $.post(webApiUrl + authenticateRoute,
        {
            bearerToken: idToken
        },
        function(data, status) {
            var sss = data.bearerToken;
            localStorage.setItem(idTokenLocalStorageKey, data.bearerToken);
            showSignedIn();
        });
}

function testAuthentication() {

    $.ajax({
        url: webApiUrl + testAuthenticationRoute,
        headers: { "Authorization" : "bearer " + getIdToken() }
    }).done(function(result) {
        document.getElementById("testAuthenticationResult").textContent = JSON.stringify(result);
    }).fail(function(xhr) {
        var text = xhr.status + " : " + xhr.statusText + " : " + xhr.responseText;
        document.getElementById("testAuthenticationResult").textContent =text;
    });

    
}
