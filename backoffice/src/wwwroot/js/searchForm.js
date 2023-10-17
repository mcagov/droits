import Choices from "choices.js";


export function initializeSearchForm() {

    new Choices('.js-search-droit-status', {
        removeItems: true,
        removeItemButton: true,
        placeholder: true,
        placeholderValue: 'Select status',
    });

    new Choices('.js-select-assigned-user', {
        allowHTML: false,
        searchEnabled: true,
        itemSelectText: 'Select',
    });

}




