// Vérifie que les données sont valides
function checkDashboardData() {
    if (!window.dashboardData ||
        !dashboardData.stations ||
        dashboardData.stations.length === 0 ||
        dashboardData.totalGum <= 0) {
        console.warn("Aucune donnée valide pour afficher le dashboard.");
        document.getElementById('gauge1')?.closest('.card')?.classList.add('d-none');
        document.getElementById('gauge2')?.closest('.card')?.classList.add('d-none');
        document.getElementById('gauge3')?.closest('.card')?.classList.add('d-none');
        document.getElementById('barChart')?.closest('.card')?.classList.add('d-none');
        return false;
    }
    return true;
}

// Renvoie un label et une couleur d'état basé sur la valeur
function getStatusLabel(value) {
    if (value < 50) return { label: "Bad", color: "red" };
    if (value < 80) return { label: "Warning", color: "orange" };
    return { label: "Good", color: "green" };
}

// Ajoute une description dynamique sous chaque jauge
function renderStatusLabel(canvasId, value) {
    const gauge = document.getElementById(canvasId);
    if (!gauge) return;

    const status = getStatusLabel(value);
    let desc = document.createElement("div");   
    desc.classList.add("gauge-label");
    desc.style.color = status.color;
    desc.style.fontWeight = "bold";
    desc.style.marginTop = "1px";
    desc.innerText = `${status.label}`;

    gauge.parentElement.appendChild(desc);
}

// Jauge graduée avec titre + légende dynamique
function renderGaugeWithGraduation(canvasId, value, label) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;

    // Crée et affiche la jauge
    new RadialGauge({
        renderTo: canvas,
        width: 250,
        height: 250,
        units: "", // Ne pas afficher % dans la jauge elle-même
        title: label,
        value: value,
        minValue: 0,
        maxValue: 100,
        majorTicks: ["0", "20", "40", "60", "80", "100"],
        minorTicks: 2,
        highlights: [
            { from: 0, to: 50, color: "#FF0000" },
            { from: 50, to: 80, color: "#FFA500" },
            { from: 80, to: 100, color: "#00C853" }
        ],
        valueBox: false, // Supprime la boîte de valeur d'origine
        colorPlate: "#fff",
        borderShadowWidth: 0,
        borders: false,
        needleType: "arrow",
        needleWidth: 3,
        animationDuration: 1500,
        animationRule: "linear"
    }).draw();

    // Ajoute les valeurs personnalisées sous la jauge
    const gaugeContainer = canvas.parentElement;

    // Nettoyage ancien contenu
    gaugeContainer.querySelectorAll(".gauge-value, .gauge-label").forEach(el => el.remove());

    // Ajoute la valeur en grand
    const valueDiv = document.createElement("div");
    valueDiv.classList.add("gauge-value", "text-center", "mt-2");
    valueDiv.style.fontSize = "1.4rem";
    valueDiv.style.fontWeight = "700";
    valueDiv.style.color = "#333";
    valueDiv.innerText = `${value.toFixed(1)}%`;
    gaugeContainer.appendChild(valueDiv);

    // Ajoute le statut
    const status = getStatusLabel(value);
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
    if (!canvas) return;

    const { labels, gumData, awtData } = window.chartData;
    // Définir dynamiquement la largeur du canvas selon le nombre de labels
    const minWidthPerStation = 80; // pixels par station
    const totalWidth = labels.length * minWidthPerStation;
    canvas.width = totalWidth;

    // Préparer les points pour scatter chart
    const scatterPoints = labels.map((label, index) => ({
        x: label,
        y: awtData[index] // ou maxAwtData[index], c’est la même chose ici
    }));

    new Chart(canvas, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'GUM',
                    data: gumData,
                    backgroundColor: '#999999', // gris
                    barThickness: 13,
                    order: 1

                },
                {
                    label: 'AWT',
                    data: awtData,
                    backgroundColor: '#66b2ff', // bleu
                    barThickness: 10,
                    order: 2

                },

                {
                    label: 'Max AWT',
                    type: 'scatter',
                    data: scatterPoints,
                    backgroundColor: 'red',
                    pointRadius: 4,
                    pointStyle: 'circle',
                    showLine: false,
                    ordre: 4
                },
                {
                    label: 'Min AWT',
                    type: 'scatter',
                    data: scatterPoints,
                    backgroundColor: 'green',
                    pointRadius: 5,
                    pointStyle: 'circle',
                    showLine: false,
                    order: 3
                },
                {
                    label: 'Tact Time',
                    type: 'line',
                    data: window.chartData.tactTimeData, 
                    borderColor:'yellow',
                    borderWidth: 1,
                    fill: false,
                    tension: 4,
                    pointRadius: 0,
                    order:5
                },
                {
                    label: 'Conveyor Speed',
                    type: 'line',
                    data: window.chartData.conveyorSpeedData, 
                    borderColor:'black',
                    borderWidth: 1,
                    fill: false,
                    tension:4,
                    pointRadius: 0,
                    order:6
                },
                {
                    label: 'Cycle Time',
                    type: 'line',
                    data: window.chartData.cycleTimeData, 
                    borderColor:'orange',
                    borderWidth: 1,
                    fill: false,
                    tension: 4,
                    pointRadius: 0,
                    order:7
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
                        autoSkip: false, // ne saute pas les noms
                        maxRotation: 50,
                        minRotation: 50,
                        font: {
                            size: 12
                        }

                    }
                },                   
                y: {
                    beginAtZero: true 
                }
            }
        },

    });
}


// === Lancement ===
if (checkDashboardData()) {
    renderGaugeWithGraduation('gauge1', dashboardData.manpowerAllocation, 'Manpower Allocation');
    renderGaugeWithGraduation('gauge2', dashboardData.pourcentageAWTvsGUM, 'AWT vs GUM');
    renderGaugeWithGraduation('gauge3', dashboardData.lineEffectiveness, 'Line Effectiveness');
    renderBarChart();
}
