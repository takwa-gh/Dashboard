function createRealGauge(canvasId, value, maxValue = 100, label = "") {
    const ctx = document.getElementById(canvasId).getContext('2d');

    // Si tu utilises Chart.js 3 ou supérieur, il faut enregistrer le plugin
    Chart.register(ChartGauge); // Enregistre le plugin gauge (ChartGauge)

    new Chart(ctx, {
        type: 'gauge',  // Utilise le type 'gauge' uniquement si le plugin est ajouté
        data: {
            datasets: [{
                value: value,
                minValue: 0,
                data: [50, 70, 90, maxValue],
                backgroundColor: ['green', 'yellow', 'orange', 'red'],
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            animation: { duration: 1000 },
            needle: {
                radiusPercentage: 2,
                widthPercentage: 3.2,
                lengthPercentage: 80,
                color: 'black'
            },
            valueLabel: {
                display: true,
                formatter: (val) => `${Math.round(val)}%`
            },
            plugins: {
                tooltip: { enabled: false },
                title: {
                    display: true,
                    text: label,
                    font: { size: 16 }
                }
            }
        }
    });
}
function initDashboardCharts(data) {
    // Jauges
    createRealGauge("gauge1", (data.totalGum !== 0 ? (data.totalAwt / data.totalGum) * 100 : 0), 150, "AWT vs GUM (%)");
    createRealGauge("gauge2", data.manpower, 200, "Total Manpower");
    createRealGauge("gauge3", data.effectiveness, 100, "Effectiveness (%)");

    // Donut chart
    new Chart(document.getElementById('donutChart'), {
        type: 'doughnut',
        data: {
            labels: ['AWT', 'Remaining to GUM'],
            datasets: [{
                data: [data.totalAwt, Math.max(0, data.totalGum - data.totalAwt)],
                backgroundColor: [data.donutColor, '#e0e0e0'],
            }]
        },
        options: {
            cutout: '70%',
            plugins: {
                tooltip: { enabled: false },
                legend: { display: true, position: 'bottom' },
            }
        }
    });

    // Bar chart : comparaison globale AWT vs GUM
    //new Chart(document.getElementById('barChart'), {
      //  type: 'bar',
      /*  data: {
            labels: ['AWT > GUM', 'AWT ≈ GUM (≥ 90%)', 'AWT < 90% GUM'],
            datasets: [{
                label: 'Nombre de Stations',
                data: [data.awtGreater, data.awtBetween, data.awtLess],
                backgroundColor: ['#ff6384', '#4bc0c0', '#ffce56']
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    stepSize: 1
                }
            }
        }
    }) ;

