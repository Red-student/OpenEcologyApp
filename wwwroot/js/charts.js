// Функция для создания графика урожайности
function createYieldChart(data) {
    const ctx = document.getElementById('yieldChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.values,
                borderColor: '#2E7D32',
                backgroundColor: 'rgba(46, 125, 50, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика урожайности',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика посевных площадей
function createAreaChart(data) {
    const ctx = document.getElementById('areaChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'pie',
        data: {
            labels: data.labels,
            datasets: [{
                data: data.values,
                backgroundColor: [
                    '#2E7D32',
                    '#4CAF50',
                    '#66BB6A',
                    '#81C784',
                    '#A5D6A7'
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Распределение посевных площадей',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика сравнения регионов
function createComparisonChart(data) {
    const ctx = document.getElementById('comparisonChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.yield,
                backgroundColor: 'rgba(46, 125, 50, 0.5)'
            }, {
                label: 'Посевная площадь (тыс. га)',
                data: data.area,
                backgroundColor: 'rgba(76, 175, 80, 0.5)'
            }, {
                label: 'Валовый сбор (тыс. т)',
                data: data.harvest,
                backgroundColor: 'rgba(102, 187, 106, 0.5)'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Сравнение регионов',
                    color: '#2E7D32'
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

// Функция для обновления всех графиков
function updateCharts(data) {
    createYieldChart(data.yield);
    createAreaChart(data.area);
    createComparisonChart(data.comparison);
}

// Функция для создания графика урожайности по годам
function createYieldByYearChart(data) {
    const ctx = document.getElementById('yieldChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.year),
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.map(item => item.yield),
                borderColor: '#2E7D32',
                backgroundColor: 'rgba(46, 125, 50, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика урожайности зерновых культур',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика урожайности по областям
function createYieldByRegionChart(data) {
    const ctx = document.getElementById('regionChart').getContext('2d');
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.map(item => item.region),
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.map(item => item.yield),
                backgroundColor: 'rgba(46, 125, 50, 0.5)',
                borderColor: '#2E7D32',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Урожайность зерновых по областям',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика посевных площадей
function createSownAreaChart(data) {
    const ctx = document.getElementById('sownAreaChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.year),
            datasets: [{
                label: 'Посевная площадь (тыс. га)',
                data: data.map(item => item.sownArea),
                borderColor: '#4CAF50',
                backgroundColor: 'rgba(76, 175, 80, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика посевных площадей',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика валового сбора
function createGrossHarvestChart(data) {
    const ctx = document.getElementById('grossHarvestChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.year),
            datasets: [{
                label: 'Валовый сбор (тыс. тонн)',
                data: data.map(item => item.grossHarvest),
                borderColor: '#66BB6A',
                backgroundColor: 'rgba(102, 187, 106, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика валового сбора',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для инициализации графиков
function initializeCharts(years, yields, areas) {
    // Очищаем предыдущие графики
    const yieldChartCanvas = document.getElementById("yieldChart");
    const areaChartCanvas = document.getElementById("areaChart");
    
    if (!yieldChartCanvas || !areaChartCanvas) {
        console.error("Не удалось найти элементы canvas");
        return;
    }

    // Уничтожаем существующие графики
    Chart.helpers.each(Chart.instances, function(instance) {
        instance.destroy();
    });

    // График урожайности
    new Chart(yieldChartCanvas, {
        type: 'line',
        data: {
            labels: years,
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: yields,
                borderColor: '#2E7D32',
                backgroundColor: 'rgba(46, 125, 50, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика урожайности по годам',
                    color: '#2E7D32'
                }
            }
        }
    });

    // График посевных площадей
    new Chart(areaChartCanvas, {
        type: 'pie',
        data: {
            labels: years,
            datasets: [{
                data: areas,
                backgroundColor: [
                    '#2E7D32',
                    '#4CAF50',
                    '#66BB6A',
                    '#81C784',
                    '#A5D6A7'
                ]
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: 'Распределение посевных площадей',
                    color: '#2E7D32'
                }
            }
        }
    });
} 