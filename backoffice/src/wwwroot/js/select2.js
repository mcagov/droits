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
    
    $("#js-select-unknown-wreck").on("change", toggleKnownWreck);
    
    toggleKnownWreck();
}

export function toggleFields(selectId, formFieldsId) {
    const selectElement = $(selectId);
    const formFields = $(formFieldsId);
    
    const showFields = selectElement.val() !== "";

    formFields.toggleClass("d-none",showFields);
}

function toggleKnownWreck(){
    const showKnownWreck = $("#js-select-unknown-wreck").val() === "True";
    $(".js-known-wreck").toggleClass("d-none", showKnownWreck);
}