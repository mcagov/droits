import $ from 'jquery';
import {validateFormTab} from "./validation";
import {initializeWreckMaterial} from './wreckMaterial.js';

export function initializeDroitForm() {
    
    initializeWreckMaterial();
    
    $('#js-select-wreck, #js-select-salvor').select2({
        width: '100%'
    });

    $("#js-select-wreck").on("change", function () {
        toggleFields("#js-select-wreck", "#js-wreck-form-fields");
        renderWreckPartial();
    });
    $("#js-select-salvor").on("change", function () {
        toggleFields("#js-select-salvor", "#js-salvor-form-fields");
        renderSalvorPartial();
    });
    
    toggleFields("#js-select-wreck", "#js-wreck-form-fields");
    toggleFields("#js-select-salvor", "#js-salvor-form-fields");
    
    $("#js-select-isolated-find").on("change", toggleIsolatedFind);

    renderWreckPartial();
    renderSalvorPartial();

    toggleIsolatedFind();

    $('.nav-item').each(function () {
        var navLink = $(this).find('button');
        validateFormTab(navLink);
    });
 
}

function renderWreckPartial() {
    renderPreviewPartial("#js-select-wreck",'.js-wreck-preview', "/Wreck/WreckViewPartial")
}

function renderSalvorPartial() {
    renderPreviewPartial("#js-select-salvor",'.js-salvor-preview', "/Salvor/SalvorViewPartial")
}



function renderPreviewPartial(selectSelector, containerSelector, endpoint) {
    const id = $(selectSelector).val();
    const container = $(containerSelector);
    container.empty();

    if (id === "") {
        return;
    }

    $.get(`${endpoint}/${id}`)
        .done(function (response) {
            const $responseElement = $('<div>').addClass('js-preview mt-2 border border-grey').append(response);
            container.append($responseElement);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.error("Error occurred during AJAX request:", errorThrown);
        });
}


export function toggleFields(selectId, formFieldsId) {
    const selectElement = $(selectId);
    const formFields = $(formFieldsId);
    
    const showFields = selectElement.val() !== "";

    formFields.toggleClass("d-none",showFields);
}

function toggleIsolatedFind(){
    const isIsolatedFind = $("#js-select-isolated-find").val() === "True";
    $(".js-known-wreck").toggleClass("d-none", isIsolatedFind);
}