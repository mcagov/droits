import 'bootstrap/dist/js/bootstrap.bundle';
import 'bootstrap/dist/js/bootstrap.min';

import {confirmDialog} from './confirmDialog.js';
import {initializeDroitForm} from './droitForm.js';

document.addEventListener('DOMContentLoaded', function() {
    window.confirm = function (message, callback) {
        confirmDialog(message, callback);
    };
    
    const droitFormElements = document.querySelectorAll('.js-droit-form');
    if (droitFormElements.length > 0) {
        initializeDroitForm();
    }
});
