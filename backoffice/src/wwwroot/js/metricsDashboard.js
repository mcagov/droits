import { createGrid } from 'ag-grid-community';
import moment from 'moment';
import {initializeOpenClosedGraph, initializeStatusGraph, initializeTriageGraph} from "./metricsGraphs";

function getRowStyle(params) {
    if (params.data.year === 'Total') {
        return { fontWeight: 'bold' };
    }
    return null;
}

const defaultColDef = {
    flex: 1,
    minWidth: 100,
    sortable: true,
    filter: true
};

function createGridOptions(columnDefs) {
    return {
        columnDefs: columnDefs,
        defaultColDef: defaultColDef,
        getRowStyle: getRowStyle
    };
}

const statusColumnDefs = [
    {headerName: 'Year', field: 'year'},
    {headerName: 'Month', field: 'group'},
    {headerName: 'Received', field: 'received'},
    {headerName: 'Acknowledged', field: 'acknowledged'},
    {headerName: 'Initial Research', field: 'initialResearch'},
    {headerName: 'Research', field: 'research'},
    {headerName: 'Salvage Award', field: 'salvageAward'},
    {headerName: 'Ready For QC', field: 'readyForQC'},
    {headerName: 'QC Approved', field: 'qcApproved'},
    {headerName: 'Closed', field: 'closed'},
];

const triageColumnDefs = [
    {headerName: 'Year', field: 'year'},
    {headerName: 'Month', field: 'group'},
    {headerName: 'Triage 1', field: 'triage1'},
    {headerName: 'Triage 2', field: 'triage2'},
    {headerName: 'Triage 3', field: 'triage3'},
    {headerName: 'Triage 4', field: 'triage4'},
    {headerName: 'Triage 5', field: 'triage5'}
];

const openClosedColumnDefs = [
    {headerName: 'Year', field: 'year'},
    {headerName: 'Month', field: 'group'},
    {headerName: 'Reported Count', field: 'reported'},
    {headerName: 'Open Count', field: 'open'},
    {headerName: 'Closed Count', field: 'closed'}
];

export function initializeMetricsDashboard() {
    // Create grid options for each grid
    const statusGridOptions = createGridOptions(statusColumnDefs);
    const triageGridOptions = createGridOptions(triageColumnDefs);
    const openClosedGridOptions = createGridOptions(openClosedColumnDefs);

    // Create the grids
    const yearStatusGrid = createGrid(document.querySelector('#year-status-grid-container'), statusGridOptions);
    const monthStatusGrid = createGrid(document.querySelector('#month-status-grid-container'), statusGridOptions);

    const yearTriageGrid = createGrid(document.querySelector('#year-triage-grid-container'), triageGridOptions);
    const monthTriageGrid = createGrid(document.querySelector('#month-triage-grid-container'), triageGridOptions);

    const yearOpenClosedGrid = createGrid(document.querySelector('#year-open-closed-grid-container'), openClosedGridOptions);
    const monthOpenClosedGrid = createGrid(document.querySelector('#month-open-closed-grid-container'), openClosedGridOptions);

    // Fetch data using AJAX
    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function() {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                const data = JSON.parse(xhr.responseText);
                const yearStatusData = [];
                const monthStatusData = [];

                const yearTriageData = [];
                const monthTriageData = [];

                const yearOpenClosedData = [];
                const monthOpenClosedData = [];

                data.forEach(function(yearData) {
                    yearData.groups.forEach(function(groupData) {
                        const statusRowData = {
                            year: yearData.year,
                            group: groupData.group,
                            received: groupData.countPerStatus["Received Count"],
                            acknowledged: groupData.countPerStatus["Acknowledged Count"],
                            initialResearch: groupData.countPerStatus["Initial Research Count"],
                            research: groupData.countPerStatus["Research Count"],
                            salvageAward: groupData.countPerStatus["Salvage Award Count"],
                            readyForQC: groupData.countPerStatus["Ready For QC Count"],
                            qcApproved: groupData.countPerStatus["QC Approved Count"],
                            closed: groupData.countPerStatus["Closed Count"],
                        };

                        const triageRowData = {
                            year: yearData.year,
                            group: groupData.group,
                            triage1: groupData.countPerTriage["Triage 1 Count"],
                            triage2: groupData.countPerTriage["Triage 2 Count"],
                            triage3: groupData.countPerTriage["Triage 3 Count"],
                            triage4: groupData.countPerTriage["Triage 4 Count"],
                            triage5: groupData.countPerTriage["Triage 5 Count"]
                        };

                        const openClosedRowData = {
                            year: yearData.year,
                            group: groupData.group,
                            reported: groupData.countPerStatus["Reported Count"],
                            open: groupData.countPerStatus["Open Count"],
                            closed: groupData.countPerStatus["Closed Count"],
                        };
                        if(groupData.group === "Total") {
                            yearStatusData.push(statusRowData);
                            yearTriageData.push(triageRowData);
                            yearOpenClosedData.push(openClosedRowData);
                        } else {
                            monthStatusData.push(statusRowData);
                            monthTriageData.push(triageRowData);
                            monthOpenClosedData.push(openClosedRowData);
                        }
                    });
                });

                yearStatusGrid.setGridOption('rowData', yearStatusData);
                monthStatusGrid.setGridOption('rowData', monthStatusData);

                yearTriageGrid.setGridOption('rowData', yearTriageData);
                monthTriageGrid.setGridOption('rowData', monthTriageData);

                yearOpenClosedGrid.setGridOption('rowData', yearOpenClosedData);
                monthOpenClosedGrid.setGridOption('rowData', monthOpenClosedData);

                // Add CSV Export
                document.querySelector('#export-csv-year-status').addEventListener('click', () => {
                    exportCsv(yearStatusGrid, "StatusYearExport");
                });
                document.querySelector('#export-csv-month-status').addEventListener('click', () => {
                    exportCsv(monthStatusGrid, "StatusMonthExport");
                });
                document.querySelector('#export-csv-year-triage').addEventListener('click', () => {
                    exportCsv(yearTriageGrid, "TriageYearExport");
                });
                document.querySelector('#export-csv-month-triage').addEventListener('click', () => {
                    exportCsv(monthTriageGrid, "TriageMonthExport");
                });
                document.querySelector('#export-csv-year-open-closed').addEventListener('click', () => {
                    exportCsv(yearOpenClosedGrid, "OpenClosedYearExport");
                });
                document.querySelector('#export-csv-month-open-closed').addEventListener('click', () => {
                    exportCsv(monthOpenClosedGrid, "OpenClosedMonthExport");
                });

                yearStatusGrid.addEventListener('filterChanged', () => drawGraph(yearStatusGrid, 'statusYearChart'));
                monthStatusGrid.addEventListener('filterChanged', () => drawGraph(monthStatusGrid, 'statusMonthChart'));
                yearTriageGrid.addEventListener('filterChanged', () => drawGraph(yearTriageGrid, 'triageYearChart'));
                monthTriageGrid.addEventListener('filterChanged', () => drawGraph(monthTriageGrid, 'triageMonthChart'));
                yearOpenClosedGrid.addEventListener('filterChanged', () => drawGraph(yearOpenClosedGrid, 'openClosedYearChart'));
                monthOpenClosedGrid.addEventListener('filterChanged', () => drawGraph(monthOpenClosedGrid, 'openClosedMonthChart'));


                // Initialize the graphs initially
                drawGraphs();

            } else {
                console.error('Failed to fetch data: ' + xhr.status);
            }
        }
    };

    xhr.open('GET', '/Account/MetricsData', true);
    xhr.send();

    function drawGraphs() {
        drawGraph(yearStatusGrid, 'statusYearChart');
        drawGraph(monthStatusGrid, 'statusMonthChart');
        drawGraph(yearTriageGrid, 'triageYearChart');
        drawGraph(monthTriageGrid, 'triageMonthChart');
        drawGraph(yearOpenClosedGrid, 'openClosedYearChart');
        drawGraph(monthOpenClosedGrid, 'openClosedMonthChart');
    }

    function drawGraph(grid, chartId) {
        const data = grid.getModel().rowsToDisplay.map(row => row.data).reverse();

        switch (chartId) {
            case 'statusYearChart':
                initializeStatusGraph(data, chartId, false, 'bar');
                break;
            case 'statusMonthChart':
                initializeStatusGraph(data, chartId, true, 'bar');
                break;
            case 'triageYearChart':
                initializeTriageGraph(data, chartId, false, 'bar');
                break;
            case 'triageMonthChart':
                initializeTriageGraph(data, chartId, false, 'line');
                break;
            case 'openClosedYearChart':
                initializeOpenClosedGraph(data, chartId, false, 'bar');
                break;
            case 'openClosedMonthChart':
                initializeOpenClosedGraph(data, chartId, true, 'bar');
                break;
            default:
                // Do nothing if chartId doesn't match any case
                break;
        }
    }


}


function exportCsv(grid, fileName = 'export') {
    const timestamp = moment().format('YYYY-MM-DD_HH-mm');
    const csvExportParams = {
        skipHeader: false,
        columnSeparator: ',',
        fileName: `${fileName}_${timestamp}.csv`
    };

    grid.exportDataAsCsv(csvExportParams);
}
