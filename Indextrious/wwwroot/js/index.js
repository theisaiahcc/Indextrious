// Code from Hyperplexed YouTube channel
// Navigates through collection-track
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

        if(window.jscolor) {
            jscolor.trigger('input change'); // Updates the color picker input value
        }

        $.ajax({
            type: "POST",
            url: $(this).attr('action'), // submit to the Create action
            data: $(this).serialize(),  // get form data
            success: function (data) {
                refreshCollections();
                $('#collection-name').val('');
                $('#collectionColor').val(''); // Clear the color picker value
                $('#myModal').modal('hide');
            },
            error: function () {
                // show an error message
                alert('An error occurred. Please try again.');
            }
        });
    });
});
