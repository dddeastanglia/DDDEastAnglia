function filter(containerClass, element) {
    var value = $(element).val();

    $(containerClass).each(function () {
        if ($(this).children("h3").text().search(value) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
}
