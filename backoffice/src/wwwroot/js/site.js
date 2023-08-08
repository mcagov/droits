import $ from 'jquery';
import 'bootstrap/dist/js/bootstrap.bundle';
import 'select2';

import {confirmDialog} from './confirmDialog.js';
import {initializeDroitForm} from './droitForm.js';

$(document).ready(function () {

    window.confirm = function (message, callback) {
        confirmDialog(message, callback);
    };

    if ($('.js-droit-form').length > 0) {
        initializeDroitForm();
    }
});
