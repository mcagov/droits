﻿import 'bootstrap/dist/js/bootstrap.bundle';
import 'bootstrap/dist/js/bootstrap';

import {confirmDialog} from './confirmDialog.js';
import {initializeDroitForm, initializeDroitView} from './droitForm.js';
import {initializeMessageBanner} from "./messageBanner";
import {initializeSearchBar} from "./searchBar";
import Choices from "choices.js";

document.addEventListener('DOMContentLoaded', function() {
    window.confirm = function (message, callback) {
        confirmDialog(message, callback);
    };
    
    const droitFormElements = document.querySelectorAll('.js-droit-form');
    if (droitFormElements.length > 0) {
        initializeDroitForm();
    }

    const droitViewElements = document.querySelectorAll('.js-droit-view-tabs');
    if (droitViewElements.length > 0) {
        initializeDroitView();
    }
    
    const droitSearchFormButton = document.querySelector('.js-toggle-droit-search');
    droitSearchFormButton.addEventListener('click', function (){
        const droitSearchForm = document.querySelector('.js-droit-search');
        droitSearchForm.classList.toggle('d-none');
    });

    initializeSearchBar();
    initializeMessageBanner();

    new Choices('.js-search-droit-status', {
        removeItems: true,
        removeItemButton: true,
        placeholder: true,
        placeholderValue: 'Select status',
    });
    
});


