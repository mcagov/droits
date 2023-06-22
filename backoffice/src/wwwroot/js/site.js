import $ from 'jquery';
import 'bootstrap';

$(document).ready(function() {
    $('#formGroupsAccordion').on('show.bs.collapse', function() {
        $(this).find('.collapse.show').collapse('hide');
    });
});
