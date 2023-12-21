export function initializeFile() {
    
    refreshFileFormIndexes();

    document.body.addEventListener('click', function(e) {
        if (e.target.matches('.js-remove-file-form')) {
            removeFileForm.call(e.target);
        } else if (e.target.matches('.js-add-file-form')) {
            addFileForm(e.target);
        }
    });
}

export function addFileForm(button) {
    
    const root = button.closest('.js-files');
    const fileFormContainer = root.querySelector('.js-files-form-container');
    if (!fileFormContainer) {
        console.error("No .js-file-form-container found for the clicked button.");
        return;
    }

    fetch('/Droit/FileFormPartial')
        .then(response => response.text())
        .then(response => {
            const responseElement = document.createElement('div');
            responseElement.innerHTML = response;

            const index = fileFormContainer.querySelectorAll('.js-file-form').length;

            responseElement.querySelectorAll('[name]').forEach(elem => {
                const name = elem.getAttribute('name');
                if (name) {
                    elem.setAttribute('name', `DroitFileForms[${index}].${name}`);
                    elem.setAttribute('id', `DroitFileForms_[${index}]__${name}`);
                }
            });

            fileFormContainer.appendChild(responseElement);
            refreshFileFormIndexes();
        });
}
function refreshFileFormIndexes() {
    const fileFormContainers = document.querySelectorAll('.js-files-form-container');

    fileFormContainers.forEach((fileFormContainer) => {
        const wmForm = fileFormContainer.closest('.js-wreck-material-form');
        
        if (!wmForm){
            console.error("Wreck Material Form not found");
            return;
        }
        
        const wmFormId = wmForm.getAttribute("js-data-wm-id");
        const wmFormName = wmForm.getAttribute("js-data-wm-name");
        
        fileFormContainer.querySelectorAll('.js-file-form').forEach((form, index) => {
             form.querySelectorAll('input, select').forEach(elem => {
                elem.setAttribute('name', elem.getAttribute('name').replace(/.*?DroitFileForms\[\d+]/, `${wmFormName}.DroitFileForms[${index}]`));
                elem.setAttribute('id', elem.getAttribute('id').replace(/.*?DroitFileForms_\[\d+]/, `${wmFormId}__DroitFileForms_[${index}]`));
            });
        });
    });
}



export function removeFileForm() {
    const confirmMessage = 'Are you sure you want to remove this File?';
    const removeAction = function () {
        this.closest('.js-file-form').remove();
        refreshImageFormIndexes();
        const confirmDialog = document.querySelector('#confirmDialog');
        const modalInstance = bootstrap.Modal.getInstance(confirmDialog);
        modalInstance.hide();
    };
    window.confirm(confirmMessage, removeAction.bind(this));
}
