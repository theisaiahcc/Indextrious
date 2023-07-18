// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

// Code from Hyperplexed YouTube channel
const track = document.getElementById("collection-track");

window.onmousedown = e => {
    track.dataset.mouseDownAt = e.clientX;
}

window.onmouseup = () => {
    track.dataset.mouseDownAt = "0";
    track.dataset.prevPercentage = track.dataset.percentage;
}

window.onmousemove = e => {
    if (track.dataset.mouseDownAt === "0") return;

    const mouseDelta = parseFloat(track.dataset.mouseDownAt) - e.clientX,
        maxDelta = window.innerWidth / 2;

    const percentage = (mouseDelta / maxDelta) * -100,
        nextPercentageUnconstrained = parseFloat(track.dataset.prevPercentage) + percentage,
        nextPercentage = Math.max(Math.min(nextPercentageUnconstrained, 0), -100);

    track.dataset.percentage = nextPercentage;

    track.animate({
        transform: `translate(${nextPercentage}%, -50%)`
    }, { duration: 1200, fill: "forwards" });
}



$(document).ready(function () {
    $('#create-collection-form').on('submit', function (e) {
        e.preventDefault();  // prevent the form from doing a full postback

        $.ajax({
            type: "POST",
            url: $(this).attr('action'), // submit to the Create action
            data: $(this).serialize(),  // get form data
            success: function (data) {
                refreshCollections();
                // close the modal
                $('#myModal').modal('hide');
            },
            error: function () {
                // show an error message
                alert('An error occurred. Please try again.');
            }
        });
    });
});

function refreshCollections() {
    $.ajax({
        type: "GET",
        url: "/Home/GetCollectionsPartial",
        success: function (data) {
            $('#collection-track').html(data);
            // Reset navigation attributes
            $('#collection-track').attr('data-mouse-down-at', '0');
            $('#collection-track').attr('data-prev-percentage', '0');
        },
        error: function () {
            // show an error message
            alert('An error occurred while refreshing the collections. Please try again.');
        }
    });
}
