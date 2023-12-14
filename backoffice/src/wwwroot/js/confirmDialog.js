let modalInstance;

document.addEventListener('DOMContentLoaded', function() {
    const modalElement = document.getElementById('confirmDialog');
    modalInstance = new bootstrap.Modal(modalElement);
});

export function confirmDialog(message, callback) {
    document.getElementById('confirmDialogTitle').textContent = 'Confirmation';
    document.getElementById('confirmDialogMessage').textContent = message;

    const confirmButton = document.getElementById('confirmDialogBtn');
    const newConfirmButton = confirmButton.cloneNode(true);
    confirmButton.parentNode.replaceChild(newConfirmButton, confirmButton);

    newConfirmButton.addEventListener('click', function() {
        modalInstance.hide();
        callback();
    });

    modalInstance.show();
}
