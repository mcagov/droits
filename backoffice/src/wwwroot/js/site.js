import $ from 'jquery';
import 'bootstrap/dist/js/bootstrap.bundle';
import 'select2';

import { confirmDialog } from './confirmDialog.js';
import { validateFormTab } from './validation.js';
import { initializeSelect2, toggleFields } from './select2.js';
import { removeWreckMaterialForm, addWreckMaterialForm } from './wreckMaterial.js';

$(function () {

    initializeSelect2();
    
    window.confirm = function (message, callback) {
        confirmDialog(message, callback);
    };
    
    $('.nav-item').each(function () {
        var navLink = $(this).find('button');
        validateFormTab(navLink);
    });


    $('body').on('click', '.js-remove-wreck-material-form', function () {
        removeWreckMaterialForm.call(this);
    });

    $('#js-add-wreck-material-form').on('click', function () {
        addWreckMaterialForm();
    });
});
