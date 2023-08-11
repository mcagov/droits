import {validateFormTab} from "./validation";
import {initializeWreckMaterial} from './wreckMaterial.js';
import Choices from "choices.js";

export function initializeDroitForm() {

    initializeWreckMaterial();

    const selectWreck = document.querySelector('#js-select-wreck');
    const selectSalvor = document.querySelector('#js-select-salvor');
    const selectIsolatedFind = document.querySelector("#js-select-isolated-find");

    new Choices('#js-select-user', {
        allowHTML: false,
        searchEnabled: true,
        itemSelectText: 'Select',
    });
    
     new Choices('#js-select-salvor', {
        allowHTML: false,
        searchEnabled: true,
        itemSelectText: 'Select',
    });

    new Choices('#js-select-wreck', {
        allowHTML: false,
        searchEnabled: true,
        itemSelectText: 'Select',
    });

    selectWreck.addEventListener("change", function() {
        toggleFields(selectWreck, document.querySelector("#js-wreck-form-fields"));
        renderWreckPartial();
    });

    selectSalvor.addEventListener("change", function() {
        toggleFields(selectSalvor, document.querySelector("#js-salvor-form-fields"));
        renderSalvorPartial();
    });

    toggleFields(selectWreck, document.querySelector("#js-wreck-form-fields"));
    toggleFields(selectSalvor, document.querySelector("#js-salvor-form-fields"));

    selectIsolatedFind.addEventListener("change", toggleIsolatedFind);

    renderWreckPartial();
    renderSalvorPartial();

    toggleIsolatedFind();

    document.querySelectorAll('.nav-item').forEach(navItem => {
        validateFormTab(navItem.querySelector('button'));
    });

}

function renderWreckPartial() {
    renderPreviewPartial("#js-select-wreck", '.js-wreck-preview', "/Wreck/WreckViewPartial");
}

function renderSalvorPartial() {
    renderPreviewPartial("#js-select-salvor", '.js-salvor-preview', "/Salvor/SalvorViewPartial");
}

function renderPreviewPartial(selectSelector, containerSelector, endpoint) {
    const id = document.querySelector(selectSelector).value;
    const container = document.querySelector(containerSelector);
    container.innerHTML = "";

    if (id === "") {
        return;
    }

    fetch(`${endpoint}/${id}`)
        .then(response => response.text())
        .then(response => {
            const responseElement = document.createElement('div');
            responseElement.classList.add('js-preview', 'mt-2', 'border', 'border-grey');
            responseElement.innerHTML = response;
            container.appendChild(responseElement);
        })
        .catch(error => {
            console.error("Error occurred during fetch request:", error);
        });
}

export function toggleFields(selectElement, formFields) {
    const showFields = selectElement.value === "";
    if (showFields) {
        formFields.classList.remove("d-none");
    } else {
        formFields.classList.add("d-none");
    }
}

function toggleIsolatedFind() {
    const isIsolatedFind = document.querySelector("#js-select-isolated-find").value === "True";
    const knownWreck = document.querySelector(".js-known-wreck");
    if (isIsolatedFind) {
        knownWreck.classList.add("d-none");
    } else {
        knownWreck.classList.remove("d-none");
    }
}
