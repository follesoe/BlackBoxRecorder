$(document).ready(function () {
    $(document).pngFix();
});
  

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

