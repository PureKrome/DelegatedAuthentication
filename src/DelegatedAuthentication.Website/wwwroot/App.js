

function initialize()
{
    if (getAuth0BearerToken() === null) {
        // No login data stored.
        showNotSignedIn();
    } else {
        showSignedIn();
    }

    initializeAuth();
}

function showNotSignedIn() {
    toggleSignInDivs("block", "none");  
}

function showSignedIn() {
    toggleSignInDivs("none", "block");
    document.getElementById("signedInIdToken").textContent = "UserId [" + getUserId() + "]<br/>" + "Auth0 Bearer: " + getAuth0BearerToken() + "<br/>Custom Bearer: " + getCustomBearerToken();
}

function toggleSignInDivs(notSignedInDivValue, signedInDivValue) {
    document.getElementById("NotSignedInDiv").style.display = notSignedInDivValue;
    document.getElementById("SignedInDiv").style.display = signedInDivValue;
}
