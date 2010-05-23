$(document).ready(function () {
    $(document).pngFix();
});
  
// Banner slide
$(document).ready(function () {
    var currentPosition = 0;
    var slideWidth = 450;
    var slides = $("#slideShow li");
    var numberOfSlides = slides.length;

    $(".content").css("overflow", "hidden");

    $("ul#slideShow").css("width", slideWidth * numberOfSlides);

    manageControls(currentPosition);

    $(".slideBtn").bind("click", function () {
        currentPosition = ($(this).attr("id") == "next") ? currentPosition + 1 : currentPosition - 1;
        manageControls(currentPosition);

        $("#slideShow").animate({
            "left": slideWidth * (-currentPosition)
        });
    });

    function manageControls(position) {
        if (position == 0) {
            $("#prev").hide();
        }
        else {
            $("#prev").show();
        }

        if (position == numberOfSlides - 1) {
            $("#next").hide();
        }
        else {
            $("#next").show();
        }
    }
});

// Tweet show and hide
$(document).ready(function () {
    $(".tweet").hover(
        function () {
            $(".twitter_div", this).stop(true, true).animate({ width: "300px", padding: "5px" }) 
        },
        function () {
            $(".twitter_div", this).animate({ width: "0", padding: "0" }) 
        });
});


// Tweets
$(document).ready(function () {

    $("#marcus").tweet({
        username: "marcusalmgren",
        join_text: "auto",
        count: 1,
        loading_text: "loading tweets..."
    });

    $("#jonas").tweet({
        username: "follesoe",
        join_text: "auto",
        count: 1,
        loading_text: "loading tweets..."
    });

    $("#alex").tweet({
        username: "alex_york",
        join_text: "auto",
        count: 1,
        loading_text: "loading tweets..."
    });

    $("#hege").tweet({
        username: "hegerokenes",
        join_text: "auto",
        count: 1,
        loading_text: "loading tweets..."
    });

});