export function initializeImage() {
    document.body.addEventListener('click', function(e) {
        if (e.target.matches('.js-remove-image-form')) {
            removeImageForm.call(e.target);
        } else if (e.target.matches('.js-add-image-form')) {
            addImageForm(e.target);
        }
    });
}

export function addImageForm(button) {
    
    const root = button.closest('.js-images');
    const imageFormContainer = root.querySelector('.js-images-form-container');
    if (!imageFormContainer) {
        console.error("No .js-images-form-container found for the clicked button.");
        return;
    }

    fetch('/Droit/ImageFormPartial')
        .then(response => response.text())
        .then(response => {
            const responseElement = document.createElement('div');
            responseElement.innerHTML = response;

            const index = imageFormContainer.querySelectorAll('.js-image-form').length;

            responseElement.querySelectorAll('[name]').forEach(elem => {
                const name = elem.getAttribute('name');
                if (name) {
                    elem.setAttribute('name', `ImageForms[${index}].${name}`);
                    elem.setAttribute('id', `ImageForms_[${index}]__${name}`);
                }
            });

            imageFormContainer.appendChild(responseElement);
            refreshImageFormIndexes();
        });
}


function refreshImageFormIndexes(containerClass) {
    const imageFormContainers = document.querySelectorAll(`.js-images-form-container`);
    
    imageFormContainers.forEach((imageFormContainer) => {
        imageFormContainer.querySelectorAll('.js-image-form').forEach((form, index) => {
            form.querySelectorAll('input, select').forEach(elem => {
                elem.setAttribute('name', elem.getAttribute('name').replace(/ImageForms\[\d+]/, `ImageForms[${index}]`));
                elem.setAttribute('id', elem.getAttribute('id').replace(/ImageForms_\[\d+]/, `ImageForms_[${index}]`));
            });
        });
    });
}


export function removeImageForm() {
    const confirmMessage = 'Are you sure you want to remove this Image?';
    const removeAction = function () {
        this.closest('.js-image-form').remove();
        refreshImageFormIndexes();
        const confirmDialog = document.querySelector('#confirmDialog');
        const modalInstance = bootstrap.Modal.getInstance(confirmDialog);
        modalInstance.hide();
    };
    window.confirm(confirmMessage, removeAction.bind(this));
}
