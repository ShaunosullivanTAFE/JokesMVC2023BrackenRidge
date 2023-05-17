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