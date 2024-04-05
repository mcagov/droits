import Chart from 'chart.js/auto';

function initializeGraph(data, containerId, metaData, stacked = false, type = 'bar') {
    const ctx = document.getElementById(containerId).getContext('2d');

    const labels = data
        .filter(entry => entry.year !== "Total")
        .map(entry => entry.group === "Total" ? `${entry.year}` : `${entry.year}-${entry.group}`);

    const datasets = Object.keys(metaData).map(key => ({
        label: metaData[key].title,
        backgroundColor: metaData[key].colour,
        borderColor: metaData[key].colour,
        borderWidth: 1,
        data: data.map(entry => entry[key])
    }));

    const chartData = {
        labels: labels,
        datasets: datasets
    };

    const options = {
        responsive: true,
        maintainAspectRatio: true,
        scales: {
            x: { stacked: stacked },
            y: {
                stacked: stacked,
                stepSize: 1
            }
        }
    };

    new Chart(ctx, {
        type: type,
        data: chartData,
        options: { scales: options.scales }
    });
}

export function initializeTriageGraph(data, containerId, stacked = false, type = 'bar') {
    const triageMetaData = {
        triage1: { title: 'Triage 1', colour: 'rgba(255, 99, 132, 0.5)' },
        triage2: { title: 'Triage 2', colour: 'rgba(54, 162, 235, 0.5)' },
        triage3: { title: 'Triage 3', colour: 'rgba(255, 206, 86, 0.5)' },
        triage4: { title: 'Triage 4', colour: 'rgba(75, 192, 192, 0.5)' },
        triage5: { title: 'Triage 5', colour: 'rgba(153, 102, 255, 0.5)' }
    };
    initializeGraph(data, containerId, triageMetaData, stacked, type);
}

export function initializeStatusGraph(data, containerId, stacked = false, type = 'bar') {
    const statusMetaData = {
        received: { title: 'Received', colour: 'rgba(255, 99, 132, 0.5)' },
        acknowledged: { title: 'Acknowledged', colour: 'rgba(54, 162, 235, 0.5)' },
        initialResearch: { title: 'Initial Research', colour: 'rgba(255, 206, 86, 0.5)' },
        research: { title: 'Research', colour: 'rgba(75, 192, 192, 0.5)' },
        salvageAward: { title: 'Salvage Award', colour: 'rgba(153, 102, 255, 0.5)' },
        readyForQC: { title: 'Ready For QC', colour: 'rgba(255, 159, 64, 0.5)' },
        qcApproved: { title: 'QC Approved', colour: 'rgba(51, 204, 51, 0.5)' },
        closed: { title: 'Closed', colour: 'rgba(75, 192, 192, 0.5)' }
    };
    initializeGraph(data, containerId, statusMetaData, stacked, type);
}

export function initializeOpenClosedGraph(data, containerId, stacked = false, type = 'bar') {
    const openClosedMetaData = {
        open: { title: 'Open Count', colour: 'rgba(255, 102, 0, 0.5)' },
        closed: { title: 'Closed Count', colour: 'rgba(51, 204, 51, 0.5)' }
    };
    initializeGraph(data.filter(entry => entry.reported > 0 && entry.year !== "Total"), containerId, openClosedMetaData, stacked, type);
}
