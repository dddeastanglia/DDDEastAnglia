function updateGravatarPicture(postUrl, emailAddress, pictureId) {
    $.post(postUrl,
        { emailAddress: emailAddress }
    ).done(function (url) {
        $('#' + pictureId).attr("src", url);
    });
}
