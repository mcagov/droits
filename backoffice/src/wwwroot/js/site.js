import 'bootstrap/dist/js/bootstrap.bundle';

// Test if Bootstrap is available
if (typeof bootstrap !== 'undefined') {
  console.log('Bootstrap is imported correctly.');
} else {
  console.log('Bootstrap is not imported correctly.');
}

$(document).ready(function() {
    $('#formTab a').on('click', function (e) {
      e.preventDefault();
      $(this).tab('show');
    });
  });
