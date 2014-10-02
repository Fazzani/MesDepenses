$(function () {
    $.getJSON(urlGetData, function (data) {
        console.debug(data);
    });

    function AppViewModel() {

        this.data = $.getJSON(urlGetData, function (data) {
            console.debug(data);
        });
    }
    
    var Operation = {
        compte : ko.observable(),
        dateCompta : ko.observable(),
        dateOp : ko.observable(),
        libelle : ko.observable(),
        refX : ko.observable(),
        dateVal : ko.observable(),
        montant : ko.observable()
    }
});