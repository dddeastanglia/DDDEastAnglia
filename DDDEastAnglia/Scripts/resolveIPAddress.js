function resolveAllIPAddresses(sender) {
    $('#' + sender.id).animate({ opacity: "0.0" });
    $('a[id^="link"]').each(function () {
        $(this).click();
    });
}

function resolveIPAddress(linkId, address, outputId, postUrl) {
    var link = $('#' + linkId);
    link.replaceWith('<i id="' + linkId + '" class="icon-spin icon-spinner"></i>');
    $.post(
        postUrl,
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
