$('div[id^="progressbar"]').each(function() {
    var maxValue = parseInt($(this).attr("data-value-max"));
    var currentValue = parseInt($(this).attr("data-value-current"));
    $(this).progressbar({
        max: maxValue,
        value: currentValue
    });
});
