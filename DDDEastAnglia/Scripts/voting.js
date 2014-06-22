
$(function () {
    $('.voting').each(function(index) {
        replaceSubmit($(this), onVoteComplete($(this)), index);
    });
    
    function replaceSubmit(voteSpan, handler, index) {
        voteSpan.find('form').submit(function (e) {
            e.preventDefault();
            var data = { width: $(window).width(), height: $(window).height(), positionInList: index };

            // Now I want to take the form's url and post directly to it if successful then change the style of the button
            $.post(
                this.action,
                data,
                handler
            );
        });
    }
    
    function onVoteComplete(voteSpan) {
        return function (response) {
            var id = voteSpan.attr('id');
            voteSpan.replaceWith(response);
            var newItem = $('#' + id);
            replaceSubmit(newItem, onVoteComplete(newItem), 0);
        };
    }
});