
$(function () {
    $('.voting').replaceSubmit($(this), onVoteComplete($(this)));
    
    function replaceSubmit(voteSpan, handler) {
        voteSpan.find('form').submit(function (e) {
            e.preventDefault();
            var data = { width: $(window).width(), height: $(window).height() };

            // Now I want to take the form's url and post directly to it if successful then change the style of the button
            $.post(
                this.action,
                data,
                onVoteComplete(voteSpan)
            );
        });
    }
    
    function onVoteComplete(voteSpan) {
        return function(response) {
            voteSpan.replaceWith(response);
        };
    }
});