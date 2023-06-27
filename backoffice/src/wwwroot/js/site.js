import $ from 'jquery';
import 'bootstrap/dist/js/bootstrap.bundle';

$(document).ready(function() {
    $("#wreck-id").on("change", function() {
        toggleWreckFields();
    });
    toggleWreckFields();
});

function toggleWreckFields() {
    var wreckIdSelect = $("#wreck-id");
    var wreckFormFields = $("#js-wreck-form-fields");

    if (wreckIdSelect.val() === "") {
        wreckFormFields.removeClass("d-none");
    } else {
        wreckFormFields.addClass("d-none");
    }
}
