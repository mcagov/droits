import Choices from "choices.js";

function initializeSearchForm(formClass, toggleButtonClass) {
    const searchFormElements = document.querySelectorAll(formClass);

    if (searchFormElements.length === 0) return;

    const searchFormButton = document.querySelectorAll(toggleButtonClass);

    searchFormButton.forEach((button) => {button.addEventListener('click', function () {
        const searchForm = document.querySelector(formClass);
        searchForm.classList.toggle('d-none');
    })});
    
    switch (formClass) {
        case '.js-droit-search':
            initializeChoices('.js-search-droit-status', 'Select Status');
            initializeChoices('.js-search-droit-recovered-from', 'Select Recovered From');
            initializeChoices('.js-select-assigned-user', 'Select Assigned To');
            break;
        case '.js-letter-search':
            initializeChoices('.js-search-letter-status', 'Select Status');
            initializeChoices('.js-search-letter-type', 'Select Type');
            break;
    }
}

function initializeChoices(selector, placeholder, searchEnabled = true, itemSelectText = 'Select') {
    new Choices(selector, {
        removeItems: true,
        removeItemButton: true,
        placeholder: true,
        placeholderValue: placeholder,
        allowHTML: false,
        searchEnabled: searchEnabled,
        itemSelectText: itemSelectText,
    });
}

export function initializeSearchForms() {
    initializeSearchForm('.js-droit-search', '.js-toggle-droit-search');
    initializeSearchForm('.js-salvor-search', '.js-toggle-salvor-search');
    initializeSearchForm('.js-wreck-search', '.js-toggle-wreck-search');
    initializeSearchForm('.js-letter-search', '.js-toggle-letter-search');
}
