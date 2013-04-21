function filter(containerClass, searchBox) {
    var searchText = $(searchBox).val();

    $(containerClass).each(function () {
        if ($(this).children("h3").text().search(searchText) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
}
