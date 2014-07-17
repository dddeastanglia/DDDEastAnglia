function expand(id, postUrl, postData) {
    $.post(postUrl, postData)
        .done(function (data) {
            var loadingIcon = $('#icon' + id);
            loadingIcon.replaceWith('<i id="icon' + id + '" class="icon-spin icon-spinner"></i>');
            var output = $('#placeholder' + id);
            output.hide();
            output.html(data);
            output.fadeIn('fast');
            var link = $('#link' + id);
            link.attr("onclick", "javascript:collapse('" + id + "', '" + postUrl + "', " + JSON.stringify(postData) + ");");
            loadingIcon = $('#icon' + id);
            loadingIcon.replaceWith('<i id="icon' + id + '" class="icon-chevron-down"></i>');
        });
}

function collapse(id, postUrl, postData) {
    var output = $('#placeholder' + id);
    output.fadeOut('fast');
    var link = $('#link' + id);
    link.attr("onclick", "javascript:expand('" + id + "', '" + postUrl + "', " + JSON.stringify(postData) + "); return false;");
    var loadingIcon = $('#icon' + id);
    loadingIcon.replaceWith('<i id="icon' + id + '" class="icon-chevron-right"></i>');
}
