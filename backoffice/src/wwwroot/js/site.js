import $ from 'jquery';
import 'bootstrap/dist/js/bootstrap.bundle';
import 'select2';

$(document).ready(function() {
    $("#js-select-wreck").on("change", function() {
        toggleWreckFields();
    });
    toggleWreckFields();

    $('#js-select-wreck').select2({
        width: '100%'
    });
});

function toggleWreckFields() {
    var wreckIdSelect = $("#js-select-wreck");
    var wreckFormFields = $("#js-wreck-form-fields");

    if (wreckIdSelect.val() === "") {
        wreckFormFields.removeClass("d-none");
    } else {
        wreckFormFields.addClass("d-none");
    }
}
