import {initializeImage} from "./Image";

export function initializeWreckMaterial() {
    refreshWmFormIndexes();
    initializeImage();

    document.body.addEventListener("change", function (e) {
        if (e.target.matches('.js-stored-salvor')) {
            toggleWreckMaterialStorageAddress(e.target);
        }
    });

    document.querySelectorAll('.js-stored-salvor').forEach(toggleWreckMaterialStorageAddress);
    
    document.body.addEventListener('click', function(e) {
        if (e.target.matches('.js-remove-wreck-material-form')) {
            removeWreckMaterialForm.call(e.target);
        }
    });

    const addWreckMaterialBtn = document.querySelector('#js-add-wreck-material-form');
    addWreckMaterialBtn.addEventListener('click', addWreckMaterialForm);
    
}

function toggleWreckMaterialStorageAddress(salvorAddressCheckbox) {
    const useSalvorAddress = salvorAddressCheckbox.checked;
    const addressContainer = salvorAddressCheckbox.closest(".js-wreck-material-form").querySelector(".js-wreck-material-address");
    if (useSalvorAddress) {
        addressContainer.classList.add("d-none");
    } else {
        addressContainer.classList.remove("d-none");
    }
}

export function removeWreckMaterialForm() {
    const confirmMessage = 'Are you sure you want to remove this Wreck Material?';
    const removeAction = function () {
        this.closest('.js-wreck-material-form').remove();
        refreshWmFormIndexes();
        const confirmDialog = document.querySelector('#confirmDialog');
        const modalInstance = bootstrap.Modal.getInstance(confirmDialog);
        modalInstance.hide();
    };
    window.confirm(confirmMessage, removeAction.bind(this));
}

export function addWreckMaterialForm() {
        const wmFormContainer = document.querySelector('#js-wreck-materials-form-container');
        fetch('/Droit/WreckMaterialFormPartial')
            .then(response => response.text())
            .then(response => {
                const responseElement = document.createElement('div');
                responseElement.innerHTML = response;

                const index = wmFormContainer.querySelectorAll('.js-wreck-material-form').length;

                responseElement.querySelectorAll('[name]').forEach(elem => {
                    const name = elem.getAttribute('name');
                    if (name) {
                        elem.setAttribute('name', `WreckMaterialForms[${index}].${name}`);
                        elem.setAttribute('id', `WreckMaterialForms_[${index}]__${name}`);
                    }
                });
                
                
                wmFormContainer.appendChild(responseElement);

                refreshWmFormIndexes();
                
            });
}


function refreshWmFormIndexes() {
    const wmFormContainer = document.querySelector('#js-wreck-materials-form-container');
    wmFormContainer.querySelectorAll('.js-wreck-material-form').forEach((form, index) => {
        form.setAttribute("js-data-wm-name", `WreckMaterialForms[${index}]`);
        form.setAttribute("js-data-wm-id", `WreckMaterialForms_[${index}]`);
        form.querySelectorAll('input, select').forEach(elem => {
            elem.setAttribute('name', elem.getAttribute('name').replace(/WreckMaterialForms\[\d+]/, `WreckMaterialForms[${index}]`));
            elem.setAttribute('id', elem.getAttribute('id').replace(/WreckMaterialForms_\[\d+]/, `WreckMaterialForms_[${index}]`));
        });
    });
}
