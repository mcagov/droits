import 'bootstrap/dist/js/bootstrap.bundle';
import 'bootstrap/dist/js/bootstrap';

import {confirmDialog} from './confirmDialog.js';
import {initializeDroitForm, initializeDroitView} from './droitForm.js';
import {initializeMessageBanner} from './messageBanner';
import {initializeSearchBar} from './searchBar';
import {initializeSearchForms} from './searchForms';
import {initializeFile} from "./droitFile";
import {initializeTinyMce} from "./tinyMCE";
import {initializeMetricsDashboard} from "./metricsDashboard";


document.addEventListener('DOMContentLoaded', function () {
    window.confirm = function (message, callback) {
        confirmDialog(message, callback);
    };

    window.confirmDelete = function (entityType, deleteUrl) {
        const confirmMessage = `Are you sure you want to delete this ${entityType}?`;
        confirmDialog(confirmMessage, function() {
            window.location.href = deleteUrl;
        });
    };
    

    initializeComponents('.js-droit-form', initializeDroitForm);
    initializeComponents('.js-droit-view-tabs', initializeDroitView);
    initializeComponents('.js-metrics-dashboard', initializeMetricsDashboard);

    initializeSearchBar();
    initializeMessageBanner();
    initializeSearchForms();
    initializeFile();
    initializeTinyMce();
});

function initializeComponents(selector, initializer) {
    const elements = document.querySelectorAll(selector);
    if (elements.length > 0) {
        initializer();
    }
}
