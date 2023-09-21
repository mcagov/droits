import 'bootstrap/dist/js/bootstrap.bundle';
import 'bootstrap/dist/js/bootstrap';

import {confirmDialog} from './confirmDialog.js';
import {initializeDroitForm} from './droitForm.js';
import {initializeMessageBanner} from "./messageBanner";
import {initializeSearchBar} from "./searchBar";
import {initializeDataTable} from "./dataTable";
import Choices from "choices.js";

document.addEventListener('DOMContentLoaded', function() {
    window.confirm = function (message, callback) {
        confirmDialog(message, callback);
    };
    
    const droitFormElements = document.querySelectorAll('.js-droit-form');
    if (droitFormElements.length > 0) {
        initializeDroitForm();
    }

    const droitSearchTableElements = document.querySelectorAll('#droits-search-table');
    if (droitSearchTableElements.length > 0) {
        initializeDataTable('#droits-search-table');
    }
    
    initializeSearchBar();
    initializeMessageBanner();

    new Choices('.js-search-droit-status', {
        removeItems: true,
        removeItemButton: true,
        placeholder: true,
        placeholderValue: 'Select status',
    });
    
});


