import $ from 'jquery';

export function initializeSelect2() {
    $('#js-select-wreck, #js-select-salvor').select2({
        width: '100%'
    });

    $("#js-select-wreck").on("change", function () {
        toggleFields("#js-select-wreck", "#js-wreck-form-fields");
    });
    $("#js-select-salvor").on("change", function () {
        toggleFields("#js-select-salvor", "#js-salvor-form-fields");
    });
    
    toggleFields("#js-select-wreck", "#js-wreck-form-fields");
    toggleFields("#js-select-salvor", "#js-salvor-form-fields");
}

export function toggleFields(selectId, formFieldsId) {
    var selectElement = $(selectId);
    var formFields = $(formFieldsId);

    if (selectElement.val() === "") {
        formFields.removeClass("d-none");
    } else {
        formFields.addClass("d-none");
    }
}