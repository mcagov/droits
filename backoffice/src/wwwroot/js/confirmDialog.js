import $ from 'jquery';

export function confirmDialog(message, callback) {
    $('#confirmDialogTitle').text('Confirmation');
    $('#confirmDialogMessage').text(message);
    $('#confirmDialogBtn').off('click').on('click', callback);
    $('#confirmDialog').modal('show');
}