import Choices from "choices.js";


export function initializeSearchForm() {
    // Initialize your Choices dropdowns
    const statusDropdown = new Choices('.js-search-droit-status', {
        removeItems: true,
        removeItemButton: true,
        placeholder: true,
        placeholderValue: 'Select status',
    });

    const userDropdown = new Choices('.js-select-assigned-user', {
        allowHTML: false,
        searchEnabled: true,
        itemSelectText: 'Select',
    });

    // Attach a click event listener to each dropdown
    statusDropdown.passedElement.element.addEventListener('click', () => {
        bringDropdownToFront(statusDropdown.passedElement.element);
    });

    userDropdown.passedElement.element.addEventListener('click', () => {
        bringDropdownToFront(userDropdown.passedElement.element);
    });
}

function bringDropdownToFront(dropdownElement) {
    // Find the parent element of the dropdown
    const parent = dropdownElement.parentElement;

    // Move the dropdown to the end of the parent's child elements
    parent.appendChild(dropdownElement);
}



