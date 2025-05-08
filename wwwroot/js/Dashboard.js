// Manpower Allocation Gauge
var ctx1 = document.getElementById('gauge1').getContext('2d');
var gauge1 = new Chart(ctx1, {
    type: 'doughnutGauge',
    data: {
        datasets: [{
            data: [@Model.ManpowerAllocation], // Utilisez la valeur ManpowerAllocation du modèle
            backgroundColor: ['#36A2EB'], // Couleur de la jauge
            borderWidth: 1,
        }]
    },
    options: {
        responsive: true,
        circumference: Math.PI,
        rotation: Math.PI,
        plugins: {
            datalabels: {
                display: true,
                formatter: function (value) {
                    return value + '%'; // Affichage du pourcentage dans la jauge
                }
            }
        }
    }
});

// AWT vs GUM Gauge
var ctx2 = document.getElementById('gauge2').getContext('2d');
var gauge2 = new Chart(ctx2, {
    type: 'doughnutGauge',
    data: {
        datasets: [{
            data: [@Model.PourcentageAWTvsGUM], // Utilisez la valeur PourcentageAWTvsGUM
            backgroundColor: ['#FF6384'], // Couleur de la jauge
            borderWidth: 1,
        }]
    },
    options: {
        responsive: true,
        circumference: Math.PI,
        rotation: Math.PI,
        plugins: {
            datalabels: {
                display: true,
                formatter: function (value) {
                    return value + '%'; // Affichage du pourcentage dans la jauge
                }
            }
        }
    }
});

// Line Effectiveness Gauge
var ctx3 = document.getElementById('gauge3').getContext('2d');
var gauge3 = new Chart(ctx3, {
    type: 'doughnutGauge',
    data: {
        datasets: [{
            data: [@Model.LineEffectiveness], // Utilisez la valeur LineEffectiveness
            backgroundColor: ['#FFCD56'], // Couleur de la jauge
            borderWidth: 1,
        }]
    },
    options: {
        responsive: true,
        circumference: Math.PI,
        rotation: Math.PI,
        plugins: {
            datalabels: {
                display: true,
                formatter: function (value) {
                    return value + '%'; // Affichage du pourcentage dans la jauge
                }
            }
        }
    }
});

// Bar Chart for visualizing different metrics
var ctx4 = document.getElementById('barChart').getContext('2d');
var barChart = new Chart(ctx4, {
    type: 'bar',
    data: {
        labels: ['Manpower Allocation', 'AWT vs GUM', 'Line Effectiveness'], // Les noms des KPI
        datasets: [{
            label: 'KPI Values',
            data: [@Model.ManpowerAllocation, @Model.PourcentageAWTvsGUM, @Model.LineEffectiveness], // Les valeurs des KPI
            backgroundColor: ['#36A2EB', '#FF6384', '#FFCD56'], // Couleurs des barres
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

