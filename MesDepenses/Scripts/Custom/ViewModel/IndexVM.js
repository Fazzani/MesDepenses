$(function () {
    console.debug(urlGetData);

    $.getJSON(urlGetData, null, function(data) {
        console.debug(data);
    });
});