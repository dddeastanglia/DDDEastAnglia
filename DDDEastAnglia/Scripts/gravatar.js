function updateGravatarPicture(postUrl, emailAddress, pictureId) {
    $.post(postUrl,
        { emailAddress: emailAddress }
    ).done(function (url) {
        $('#' + pictureId).attr("src", url);
    });
}

(function($) {
    $.fn.gravatarPreview = function() {
        return this.each(function () {
            $(this).unbind('mouseenter mousemove mouseleave');

            $(this).hover(function() {
                var me = $(this);
                var src = me.attr('src');
                src = src.replace(/\?s=\d+/g, "?s=300");
                $("body").after($('<img />').attr('src', src).attr('id', 'fullSizeGravatar'));

                var offset = me.position();
                var left = offset.left - window.scrollX + me.width() + 10;
                var top = offset.top - window.scrollY;

                $('#fullSizeGravatar').css({
                    position: 'fixed',
                    'z-index': 99,
                    left: left,
                    top: top,
                    display: 'none',
                    border: '1px solid black',
                    padding: '4px',
                    backgroundColor: 'white'
                });

                $('#fullSizeGravatar').fadeIn('slow');
            }, function() {
                $('#fullSizeGravatar').fadeOut('fast');
            });
        });
    };
})(jQuery);

$(document).ready(function () {
    var gravatarImages = $('img[src*="gravatar.com"]');
    gravatarImages.gravatarPreview();
    gravatarImages.css({
        cursor: 'help'
    });
});
