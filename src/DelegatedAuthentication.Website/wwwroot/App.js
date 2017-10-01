

function initialize()
{
    if (getIdToken() === null) {
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
    document.getElementById("signedInIdToken").textContent = getIdToken();
}

function toggleSignInDivs(notSignedInDivValue, signedInDivValue) {
    document.getElementById("NotSignedInDiv").style.display = notSignedInDivValue;
    document.getElementById("SignedInDiv").style.display = signedInDivValue;
}