import DataTable from 'datatables.net';
import 'datatables.net-buttons/js/dataTables.buttons.min';
import 'datatables.net-buttons/js/buttons.html5.min';
import 'datatables.net-buttons/js/buttons.print.min';
import 'datatables.net-buttons/js/buttons.colVis.min';

function initializeDataTable(selector) {
    const table = document.querySelector(selector);
    if (table) {
        const dataTable = new DataTable(table, {
            // DataTables configuration options
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'colvis',
                    text: 'Columns ',
                    className: 'btn btn-primary', // Apply Bootstrap classes
                },
                {
                    extend: 'collection',
                    text: 'Export ',
                    collectionLayout: 'dropdown',
                    buttons: [
                    {
                        extend: 'copy',
                        className: 'btn btn-primary',
                        exportOptions: {
                            columns: ':visible',
                        },
                    },
                    {
                        extend: 'csv',
                        className: 'btn btn-primary',
                        exportOptions: {
                            columns: ':visible',
                        },
                    },
                    ],
                    className: 'btn btn-primary'
                },
                
                {
                    extend: 'pageLength',
                    className: 'btn btn-primary', 
                    exportOptions: {
                        columns: ':visible', 
                    },
                },
            ],
            columnDefs: [
                {
                    targets: '_all', // Apply to all columns
                    visible: true, // Initially visible
                    searchable: true, // Allow searching
                }
            ],
            pagingType: 'full_numbers',
            pageLength: 20,
            language: {
                paginate: {
                    next: 'Next', // Customize the "Next" button text
                    previous: 'Previous', // Customize the "Previous" button text
                },
            },
        });

        // Enable individual column searching inputs
        dataTable.columns().every(function () {
            const that = this;

            // Create a search input element for each column
            const input = document.createElement('input');
            input.className = 'form-control form-control-sm mt-2';
            input.placeholder = 'Search';
            input.addEventListener('click', function (e) {
                e.stopPropagation(); // Prevent the input click event from propagating
            });

            input.addEventListener('input', function (e) {
                e.stopPropagation(); // Prevent the input click event from propagating
                that.search(this.value).draw();
            });

            // Append the search input to the column header
            that.header().appendChild(input);
        });
    }
}

export { initializeDataTable };
