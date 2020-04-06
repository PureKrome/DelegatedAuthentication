// *******************************************************************************************************
// *******************************************************************************************************
// *******************************************************************************************************

// PLEASE SET THE FOLLOWING MISSING INFORMATION.

// WebApi Server which will create the new account and return the new token.
var webApiUrl = "http://localhost:54077/";
var authenticateRoute = "Authenticate";
var testAuthenticationRoute = "AuthenticateTest";

// AUTH0 CREDENTIALS.
//var auth0ClientId = "Place Your Auth0 ClientId in here";
//var auth0Domain = "<place your auth0 tenant here>.au.auth0.com";
var auth0ClientId = "DvNbfkkR3C42i1oUwb5McbOnji5WBnvz";
var auth0Domain = "hornet-wd-dev.au.auth0.com";

// *******************************************************************************************************
// *******************************************************************************************************
// *******************************************************************************************************




var lock;
var auth0BearerTokenLocalStorageKey = "Auth0BearerToken";
var customBearerTokenLocalStorageKey = "customBearerToken";
var userIdLocalStorageKey = "userId";

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

    clearLocalStorage();

    lock.logout({
        returnTo: "http://localhost:49497"
    });
}

function getAuth0BearerToken() {
    return localStorage.getItem(auth0BearerTokenLocalStorageKey);
}

function setAuth0BearerToken(data) {
    localStorage.setItem(auth0BearerTokenLocalStorageKey, data);
}

function removeAuth0BearerToken() {
    localStorage.removeItem(auth0BearerTokenLocalStorageKey);
}

function getCustomBearerToken() {
    return localStorage.getItem(auth0BearerTokenLocalStorageKey);
}

function setCustomBearerToken(data) {
    localStorage.setItem(auth0BearerTokenLocalStorageKey, data);
}

function removeCustomBearerToken() {
    localStorage.removeItem(auth0BearerTokenLocalStorageKey);
}

function getUserId() {
    return localStorage.getItem(userIdLocalStorageKey);
}

function setUserId(data) {
    localStorage.setItem(userIdLocalStorageKey, data);
}

function removeUserId() {
    localStorage.removeItem(userIdLocalStorageKey);
}

function clearLocalStorage() {
    removeAuth0BearerToken();
    removeCustomBearerToken();
    removeUserId();
}

function authenticateWithServer(auth0BearerToken) {

    $.post(webApiUrl + authenticateRoute,
        {
            bearerToken: auth0BearerToken
        },
        function(data, status) {
            setAuth0BearerToken(auth0BearerToken);
            setCustomBearerToken(data.bearerToken);
            setUserId(data.userId);
            showSignedIn();
        });
}

function testAuthentication() {

    $.ajax({
        url: webApiUrl + testAuthenticationRoute,
        headers: { "Authorization": "bearer " + getCustomBearerToken() }
    }).done(function(result) {
        document.getElementById("testAuthenticationResult").textContent = JSON.stringify(result);
    }).fail(function(xhr) {
        var text = xhr.status + " : " + xhr.statusText + " : " + xhr.responseText;
        document.getElementById("testAuthenticationResult").textContent = text;
    });

    
}
