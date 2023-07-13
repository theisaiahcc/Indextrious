// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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