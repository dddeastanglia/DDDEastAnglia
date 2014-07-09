function filter(containerClass, searchBox, childSelector) {
    childSelector = childSelector || "h3";
    var searchText = $(searchBox).val();

    $(containerClass).each(function () {
        var sessionTitle = $(this).children(childSelector).text();
        var indexOfSearchText = sessionTitle.search(new RegExp(searchText, "i"));

        if (indexOfSearchText > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
}
