$(document).ready(function() {
    function showTooltip(x, y, contents) {
        $("<div id='tooltip'>" + contents + "</div>").css({
            position: "absolute",
            display: "none",
            top: y - 30,
            left: x + 5,
            border: "1px solid #444",
            padding: "2px",
            "background-color": "#00C",
            color: "white",
            opacity: 0.90
        }).appendTo("body").fadeIn(200);
    }

    var previousPoint = null;

    $("#chart").bind("plothover", function(event, pos, item) {
        if (item) {
            if (previousPoint != item.dataIndex) {
                previousPoint = item.dataIndex;

                $("#tooltip").remove();
                var numberOfVotes = item.datapoint[1];
                showTooltip(item.pageX, item.pageY, numberOfVotes);
            }
        } else {
            $("#tooltip").remove();
            previousPoint = null;
        }
    });
});
