import $ from 'jquery';

export function validateFormTab(navLink) {
    if ($(navLink.attr('data-bs-target')).find(".input-validation-error").length > 0) {
        navLink.addClass("text-danger");
    }
}