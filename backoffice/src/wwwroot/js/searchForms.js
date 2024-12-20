import Choices from "choices.js";
import {Input} from "postcss";

function initializeSearchForm(formClass, toggleButtonClass) {
    const searchFormElements = document.querySelectorAll(formClass);

    if (searchFormElements.length === 0) return;

    const searchFormButton = document.querySelectorAll(toggleButtonClass);

    searchFormButton.forEach((button) => {button.addEventListener('click', function () {
        const searchForm = document.querySelector(formClass);
        searchForm.classList.toggle('d-none');
    })});
    
    const sortButtons = document.querySelectorAll(".sort-link");
    
    if (sortButtons.length > 0) {
        const orderColumnField = document.querySelector(".js-order-column-field");
        const orderDescendingField = document.querySelector(".js-order-descending-field");
        const sortArrow = document.createElement("span");
        sortArrow.className = "sort-arrow";
        sortArrow.textContent = orderDescendingField.checked ? "\u2193" : "\u2191";

        sortButtons.forEach((button) => {
            const currentButtonDataField = button.getAttribute("data-sort-col");

            if (orderColumnField.value === currentButtonDataField) {
                button.appendChild(sortArrow);
            }

            button.addEventListener('click', (ev) => {
                ev.preventDefault();

                if (orderColumnField.value === currentButtonDataField) {
                    orderDescendingField.checked = !orderDescendingField.checked;
                } else {
                    orderDescendingField.checked = true;
                }

                orderColumnField.value = currentButtonDataField;

                orderColumnField.closest("form").submit();
            })
        })
    }

    const paginationButtons = document.querySelectorAll(".js-page-link");
    paginationButtons.forEach((button) => {
        button.addEventListener('click', function (ev) {
            ev.preventDefault();
            
            const paginationContainer = button.closest(".pagination-container");
            
            let inputSelector = ".js-page-number-field";
            
            if(paginationContainer) {
                inputSelector = paginationContainer.getAttribute("data-pagination-input-selector") || ".js-page-number-field";
            }

            const pageNumberField = document.querySelector(inputSelector);
            
            pageNumberField.value = button.getAttribute("data-page-number");

            pageNumberField.closest("form").submit();
        });
    });

    document.querySelectorAll('.toggleExportFields').forEach(function(button) {
        button.addEventListener("click", function(e) {
            e.preventDefault();
            const parentContainerSelector = this.getAttribute('data-parent-container');
            const checkedState = this.getAttribute('data-checked') === 'true';

            document.querySelectorAll(parentContainerSelector + ' .form-check-input').forEach(function(checkbox) {
                checkbox.checked = checkedState;
            });
        });
    });
    
    switch (formClass) {
        case '.js-droit-search':
            initializeChoices('.js-search-droit-status', 'Select Status');
            initializeChoices('.js-search-droit-triage-numbers', 'Select Triage Numbers');
            initializeChoices('.js-search-droit-recovered-from', 'Select Recovered From');
            initializeChoices('.js-select-assigned-user', 'Select Assigned To');
            initializeChoices('.js-search-droit-outcomes', 'Select Outcomes');

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
    
    initializeSearchForm('.js-dashboard-search', null);
    initializeSearchForm('.js-user-search', null);

}
