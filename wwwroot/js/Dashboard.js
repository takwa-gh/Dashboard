// Vérifie que les données sont valides
function checkDashboardData() {
    if (!window.dashboardData ||
        !dashboardData.stations ||
        dashboardData.stations.length === 0 ||
        dashboardData.totalGum <= 0) {
        console.warn("Aucune donnée valide pour afficher le dashboard.");
        ['gauge1', 'gauge2', 'gauge3', 'barChart'].forEach(id => {
            const card = document.getElementById(id)?.closest('.card');
            if (card) card.classList.add('d-none');
        });
        return false;
    }
    return true;
}

// Renvoie un label et une couleur d'état basé sur la valeur
function getStatusLabel(value, thresholds = { bad: 15, warning: 100, mode: "lessThan" }) {
    if (thresholds.mode === "between") {
        if (value < thresholds.bad) return { label: "Bad", color: "#FFCC00" };
        if (value < thresholds.warning) return { label: "Good", color: "#00C853" };
        return { label: "Bad", color: "#FF0000" };
    } else {
        return value < thresholds.bad
            ? { label: "Good", color: "#00C853" }
            : { label: "Bad", color: "#FF0000" };
    }
}
// Jauge graduée avec titre + légende dynamique
function drawGauge(canvasId, value, label, options) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;

    const { min, max, ticks, highlights, thresholds } = options;

    new RadialGauge({
        renderTo: canvas,
        width: 250,
        height: 250,
        units: "",
        title: label,
        value: value,
        minValue: min,
        maxValue: max,
        majorTicks: ticks,
        minorTicks: 2,
        highlights: highlights,
        valueBox: false,
        colorPlate: "#fff",
        borderShadowWidth: 0,
        borders: false,
        needleType: "arrow",
        needleWidth: 3,
        animationDuration: 1500,
        animationRule: "linear"
    }).draw();

    const gaugeContainer = canvas.parentElement;
    gaugeContainer.querySelectorAll(".gauge-value, .gauge-label").forEach(el => el.remove());

    const valueDiv = document.createElement("div");
    valueDiv.classList.add("gauge-value", "text-center", "mt-2");
    valueDiv.style.fontSize = "1.4rem";
    valueDiv.style.fontWeight = "700";
    valueDiv.style.color = "#333";
    valueDiv.innerText = `${value.toFixed(2)}%`;
    gaugeContainer.appendChild(valueDiv);

    const status = getStatusLabel(value, thresholds);
    const statusDiv = document.createElement("div");
    statusDiv.classList.add("gauge-label", "text-center");
    statusDiv.style.fontSize = "20px";
    statusDiv.style.fontWeight = "700";
    statusDiv.style.color = status.color;
    statusDiv.innerText = status.label;
    gaugeContainer.appendChild(statusDiv);
}

function renderBarChart() {
    const canvas = document.getElementById('barChart');
    if (!canvas || !window.chartData) return;

    const {
        labels,
        gumAverageData,
        awtAverageData,
        gumMaxData,
        gumMinData,
        awtMaxData,
        awtMinData,
        tactTimeData,
        conveyorSpeedData,
        cycleTimeData
    } = window.chartData;

    const tactTime = tactTimeData?.[0] ?? 0;
    canvas.width = labels.length * 80;

    const flectuationRateAwt = awtMaxData.map((max, i) => {
        const min = awtMinData[i];
        return min !== 0 ? +(((max - min) / min) * 100).toFixed(2) : 0;
    });

    const flectuationRateGum = gumMaxData.map((max, i) => {
        const min = gumMinData[i];
        return min !== 0 ? +(((max - min) / min) * 100).toFixed(2) : 0;
    });

    const highlightedBorderColors = awtAverageData.map(avg => avg > tactTime ? 'red' : 'rgba(0,0,0,0)');
    const highlightedBorderWidth = awtAverageData.map(avg => avg > tactTime ? 2 : 0);

    new Chart(canvas, {
        type: 'bar',
        data: {
            labels,
            datasets: [
                {
                    label: 'AVG GUM',
                    data: gumAverageData,
                    backgroundColor: '#999999',
                    barThickness: 10,
                    order: 1
                },
                {
                    label: 'AVG AWT',
                    data: awtAverageData,
                    backgroundColor: '#66b2ff',
                    borderColor: highlightedBorderColors,
                    borderWidth: highlightedBorderWidth,
                    barThickness: 8,
                    order: 2
                },
                {
                    label: 'Max AWT',
                    type: 'scatter',
                    data: awtMaxData,
                    backgroundColor: 'green',
                    pointRadius: 5,
                    order: 3
                },
                {
                    label: 'Min AWT',
                    type: 'scatter',
                    data: awtMinData,
                    backgroundColor: 'red',
                    pointRadius: 4,
                    order: 4
                },
                {
                    label: 'Tact Time',
                    type: 'line',
                    data: tactTimeData,
                    borderColor: 'green',
                    borderWidth: 2,
                    fill: false,
                    tension: 0,
                    pointRadius: 0,
                    order: 5
                },
                {
                    label: 'Conveyor Speed',
                    type: 'line',
                    data: conveyorSpeedData,
                    borderColor: 'black',
                    borderWidth: 2,
                    borderDash: [5, 5],
                    fill: false,
                    tension: 0,
                    pointRadius: 0,
                    order: 6
                },
                {
                    label: 'Cycle Time',
                    type: 'line',
                    data: cycleTimeData,
                    borderColor: 'orange',
                    borderWidth: 2,
                    borderDash: [10, 5],
                    fill: false,
                    tension: 0,
                    pointRadius: 0,
                    order: 7
                },
                {
                    label: 'Flectuation Rate AWT',
                    type: 'scatter',
                    data: flectuationRateAwt.map((y, i) => ({ x: labels[i], y })),
                    backgroundColor: 'black',
                    pointRadius: 5,
                    pointStyle: 'triangle',
                    yAxisID: 'yRight',
                    order: 8
                },
                {
                    label: 'Flectuation Rate GUM',
                    type: 'scatter',
                    data: flectuationRateGum.map((y, i) => ({ x: labels[i], y })),
                    backgroundColor: 'yellow',
                    pointRadius: 5,
                    pointStyle: 'triangle',
                    yAxisID: 'yRight',
                    order: 9
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    categoryPercentage: 0.3,
                    barPercentage: 0.4,
                    ticks: {
                        autoSkip: false,
                        maxRotation: 50,
                        minRotation: 50,
                        font: { size: 12 }
                    }
                },
                y: {
                    beginAtZero: true
                },
                yRight: {
                    position: 'right',
                    beginAtZero: true,
                    max: 100,
                    grid: { drawOnChartArea: false },
                    ticks: {
                        callback: value => `${value}%`
                    }
                }
            },
            plugins: {
                legend: {
                    labels: { usePointStyle: true }
                }
            }
        }
    });
}
// Fonction d'initialisation des jauges
function initGauges() {
    if (!checkDashboardData()) return;

    drawGauge('gauge1', dashboardData.manpowerAllocation, 'Manpower Allocation', {
        min: 0,
        max: 120,
        ticks: ["0", "20", "40", "60", "80", "100", "120"],
        highlights: [
            { from: 0, to: 80, color: "#FFCC00" },
            { from: 80, to: 110, color: "#00C853" },
            { from: 110, to: 120, color: "#FF0000" }
        ],
        thresholds: { bad: 80, warning: 110, mode: "between" }
    });

   drawGauge('gauge2', dashboardData.pourcentageAWTvsGUM, 'AWT vs GUM', {
        min: 0,
        max: 120,
        ticks: ["0", "20", "40", "60", "80", "100", "120"],
        highlights: [
            { from: 0, to: 90, color: "#FFCC00" },
            { from: 90, to: 100, color: "#00C853" },
            { from: 100, to: 120, color: "#FF0000" }
        ],
        thresholds: { bad: 90, warning: 100, mode: "between" }
    });

    drawGauge('gauge3', dashboardData.lineEffectiveness, 'Line Effectiveness', {
        min: 0,
        max: 50,
        ticks: ["0", "5", "10", "15", "20", "25", "30", "35", "40", "45", "50"],
        highlights: [
            { from: 0, to: 15, color: "#00C853" },
            { from: 15, to: 50, color: "#FF0000" }
        ],
        thresholds: { bad: 15 }
    });
}

// Fonction d'initialisation complète au chargement initial
document.addEventListener('DOMContentLoaded', function () {
    if (checkDashboardData()) {
        initGauges();        // Rendu des jauges
        renderBarChart();    // Rendu du bar chart
    }
});


