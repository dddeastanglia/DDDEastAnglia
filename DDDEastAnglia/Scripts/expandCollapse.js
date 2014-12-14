function expand(id, url, requestData) {
    $.get(url, requestData)
        .done(function (data) {
            var loadingIcon = $('#icon' + id);
            loadingIcon.replaceWith('<i id="icon' + id + '" class="icon-spin icon-spinner"></i>');
            var output = $('#placeholder' + id);
            output.hide();
            output.html(data);
            output.fadeIn('fast');
            var link = $('#link' + id);
            link.attr("onclick", "javascript:collapse('" + id + "', '" + url + "', " + JSON.stringify(requestData) + ");");
            loadingIcon = $('#icon' + id);
            loadingIcon.replaceWith('<i id="icon' + id + '" class="icon-chevron-down"></i>');
        });
}

function collapse(id, url, requestData) {
    var output = $('#placeholder' + id);
    output.fadeOut('fast');
    var link = $('#link' + id);
    link.attr("onclick", "javascript:expand('" + id + "', '" + url + "', " + JSON.stringify(requestData) + "); return false;");
    var loadingIcon = $('#icon' + id);
    loadingIcon.replaceWith('<i id="icon' + id + '" class="icon-chevron-right"></i>');
}
