// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function advFetch(url, options) {

    let verifyToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    // define the options object if it has not been defined
    if (options == undefined) {
        options = {};
    }

    // define the headers within options
    if (options.headers == undefined) {
        options.headers = {};
    }

    // set the CSRF token if found
    if (verifyToken != undefined) {
        options.headers['RequestVerificationToken'] = verifyToken;
    }

    // set a custom header to note a fetch request
    options.headers['x-fetch-request'] = "";


    var promise = fetch(url, options)
    return promise;

}

function showToast(text, type) {

    let backgroundColour;

    switch (type) {
        case 'success':
            backgroundColour = "linear-gradient(to right, #11b816, #68ed6c)"
            break;
        case 'error':
            backgroundColour = "linear-gradient(to right, #ab1f07, #cc6516)"
            break;
        default:
            backgroundColour = "linear-gradient(to right, #0f55d6, #4b97d1)"
            break;
    }

    Toastify({
        text: text,
        duration: 3000,
        newWindow: true,
        gravity: "bottom", // `top` or `bottom`
        position: "left", // `left`, `center` or `right`
        style: {
            background: backgroundColour,
        }
    }).showToast();

}


// Added
function closeModal() {
    var container = document.getElementById("modal-container")
    var backdrop = document.getElementById("modal-backdrop")
    var modal = document.getElementById("modal")

    modal.classList.remove("show")
    backdrop.classList.remove("show")

    setTimeout(function () {
        container.removeChild(backdrop)
        container.removeChild(modal)
    }, 200)
}