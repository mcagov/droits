import $ from 'jquery';
import 'bootstrap/dist/js/bootstrap.bundle';
import 'select2';

$(function() {

    window.confirm = function (message, callback) {
        $('#confirmDialogTitle').text('Confirmation');
        $('#confirmDialogMessage').text(message);
        $('#confirmDialogBtn').off('click').on('click', callback);
        $('#confirmDialog').modal('show');
    };


    $("#js-select-wreck").on("change", function() {
        toggleWreckFields();
    });
    toggleWreckFields();

    $('#js-select-wreck').select2({
        width: '100%'
    });

    var wmFormContainer = $('#js-wreck-materials-form-container');
    var wmFormAddButton = $('#js-add-wreck-material-form');

    wmFormAddButton.on('click', function () {
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
    });

    $('body').on('click','.js-remove-wreck-material-form', function () {
        var confirmMessage = 'Are you sure you want to remove this Wreck Material?';
        var removeAction = function () {
          $(this).closest('.js-wreck-material-form').remove();
          refreshWmFormIndexes();
          $('#confirmDialog').modal('hide');
        };
        confirm(confirmMessage, removeAction.bind(this));
      });

    function toggleWreckFields() {
        var wreckIdSelect = $("#js-select-wreck");
        var wreckFormFields = $("#js-wreck-form-fields");

        if (wreckIdSelect.val() === "") {
            wreckFormFields.removeClass("d-none");
        } else {
            wreckFormFields.addClass("d-none");
        }
    }

    function refreshWmFormIndexes() {
        $('#js-wreck-materials-form-container').find('.js-wreck-material-form').each(function (index) {
            $(this).find('input, select').each(function () {
                $(this).attr('name', $(this).attr('name').replace(/WreckMaterialForms\[\d+\]/, `WreckMaterialForms[${index}]`));
                $(this).attr('id', $(this).attr('id').replace(/WreckMaterialForms_\[\d+\]/, `WreckMaterialForms_[${index}]`));
            });
        });
    }
});
