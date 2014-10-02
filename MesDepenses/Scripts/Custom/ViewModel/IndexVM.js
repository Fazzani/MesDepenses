var urlGetData = '@Url.Action("ReadData", "Home")';
var ViewModel = function () {
    var self = this;

    self.items = ko.observableArray();

    self.compteLength = ko.computed(function () {
        return self.items().length;
    }, this);

    self.load = function () {
        $.getJSON(urlGetData, function (data) {
            ko.mapping.fromJS(JSON.parse(data), {}, self.items);
        });
    };
    self.load();
};

//ko.applyBindings(new ViewModel());