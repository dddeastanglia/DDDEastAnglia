function filter(containerClass, searchBox) {
    var searchText = $(searchBox).val();

    $(containerClass).each(function () {
        var sessionTitle = $(this).children("h3").text();
        var indexOfSearchText = sessionTitle.search(new RegExp(searchText, "i"));

        if (indexOfSearchText > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
}
