function resolveAllIPAddresses(sender) {
    $('#' + sender.id).animate({ opacity: "0.0" });
    $('a[id^="iplink"]').each(function () {
        $(this).click();
    });
}

function resolveIPAddress(linkId, address, outputId, url) {
    var link = $('#' + linkId);
    link.replaceWith('<i id="' + linkId + '" class="icon-spin icon-spinner"></i>');
    $.get(
        url,
        { ipAddress: address }
    ).done(function (data) {
        var loadingIcon = $('#' + linkId);
        loadingIcon.replaceWith('<i class="icon-globe"></i>');
        var output = $('#' + outputId);
        output.hide();
        output.text(data);
        output.fadeIn('fast');
    });
}
