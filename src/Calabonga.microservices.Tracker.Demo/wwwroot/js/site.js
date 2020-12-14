// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function executeRequest(options) {

    const defaultOptions = Object.assign({ elementId: "result" }, options);

    // 1. Create a new XMLHttpRequest object
    let xhr = new XMLHttpRequest();

    // 2. Configure it: GET-request for the URL /article/.../load
    xhr.open('GET', '/api/activities');
//    xhr.setRequestHeader("X-Microservice-Tracker-Id", "TEST ACTIVITY HEADER");
    xhr.setRequestHeader("X-Custom-Request-Trace-ID", "TEST ACTIVITY HEADER");

    // 3. Send the request over the network
    xhr.send();

    // 4. This will be called after the response is received
    xhr.onload = function () {
        if (xhr.status != 200) { // analyze HTTP status of the response
            alert(`Error ${xhr.status}: ${xhr.statusText}`); // e.g. 404: Not Found
        } else { // show the result
            var element = document.getElementById(defaultOptions.elementId);
            element.innerText = `Done, got ${xhr.response.length} bytes`;
            //alert(`Done, got ${xhr.response.length} bytes`); // responseText is the server
        }
    };

    //    xhr.onprogress = function (event) {
    //        if (event.lengthComputable) {
    //            alert(`Received ${event.loaded} of ${event.total} bytes`);
    //        } else {
    //            alert(`Received ${event.loaded} bytes`); // no Content-Length
    //        }
    //    };

    xhr.onerror = function () {
        alert("Request failed");
    };
}