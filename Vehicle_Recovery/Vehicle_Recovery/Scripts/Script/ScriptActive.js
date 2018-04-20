function Active(n) {
    var tab = document.getElementById(n);
    tab.className = "active";
}

function ShowSignInSignUp(n) {
    var forms = document.getElementsByClassName("signin-signup");
    var len = forms.length;
    var i = (n + len) % len;
    forms[i].style.display = "block";
}

function HiddenSignInSignUp(n) {
    var forms = document.getElementsByClassName("signin-signup");
    var len = forms.length;
    var i = (n + len) % len;
    forms[i].style.display = "none";
}

function ShowCenterSigninSignup(n) {
    var centersigninsignup = document.getElementById("background-signin-signup");
    centersigninsignup.style.display = "block";
    ShowSignInSignUp(n);
}
function HiddenCenterSigninSignup() {
    var centersigninsignup = document.getElementById("background-signin-signup");
    centersigninsignup.style.display = "none";
    var forms = document.getElementsByClassName("signin-signup");
    for (var i = 0; i < forms.length; i++) {
        forms[i].style.display = "none";
    }
}
function ShowHiddenSignInSignUp(n) {
    HiddenSignInSignUp(n + 1);
    ShowSignInSignUp(n);
}

function ShowShoppingCardPartial() {
    var background = document.getElementById("background-shopping-card-partial");
    var main = document.getElementById("info-shopping-card-partial");
    main.style.display = background.style.display = "block";
}

function HiddenShoppingCardPartial() {
    var background = document.getElementById("background-shopping-card-partial");
    var main = document.getElementById("info-shopping-card-partial");
    main.style.display = background.style.display = "none";
}