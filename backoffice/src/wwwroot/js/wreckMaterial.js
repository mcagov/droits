import $ from 'jquery';

export function initializeWreckMaterial(){

    $("body").on("change", ".js-stored-salvor", function (e) {
        toggleWreckMaterialStorageAddress(e.target);
    });
}

function toggleWreckMaterialStorageAddress(salvorAddressCheckbox){
    const useSalvorAddress = $(salvorAddressCheckbox).prop("checked");
    const addressContainer = $(salvorAddressCheckbox).closest(".js-wreck-material-form").find(".js-wreck-material-address");
    addressContainer.toggleClass("d-none", useSalvorAddress);
}

export function removeWreckMaterialForm() {
    var confirmMessage = 'Are you sure you want to remove this Wreck Material?';
    var removeAction = function () {
        $(this).closest('.js-wreck-material-form').remove();
        refreshWmFormIndexes();
        $('#confirmDialog').modal('hide');
    };
    confirm(confirmMessage, removeAction.bind(this));
}

export function addWreckMaterialForm() {
    var wmFormContainer = $('#js-wreck-materials-form-container');
    $.get('/Droit/WreckMaterialFormPartial')
        .done(function (response) {
            var index = wmFormContainer.find('.js-wreck-material-form').length;
            var $response = $(response);

            $response.find('[name]').each(function () {
                var name = $(this).attr('name');
                if (name) {
                    $(this).attr('name', `WreckMaterialForms[${index}].${name}`);
                    $(this).attr('id', `WreckMaterialForms_[${index}]__${name}`);
                }
            });

            wmFormContainer.append($response);
            refreshWmFormIndexes();
        });
}

function refreshWmFormIndexes() {
    $('#js-wreck-materials-form-container').find('.js-wreck-material-form').each(function (index) {
        $(this).find('input, select').each(function () {
            $(this).attr('name', $(this).attr('name').replace(/WreckMaterialForms\[\d+\]/, `WreckMaterialForms[${index}]`));
            $(this).attr('id', $(this).attr('id').replace(/WreckMaterialForms_\[\d+\]/, `WreckMaterialForms_[${index}]`));
        });
    });
}