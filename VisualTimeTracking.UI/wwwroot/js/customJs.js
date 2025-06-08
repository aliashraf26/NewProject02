function showModal(id) {
    $("#" + id).modal("show");
}
function hideModal() {
    $(".modal").modal("hide");
    $(".modal-backdrop").hide();
}

function initLineChart(id, name, xTitle, yTitle, series, categories, isCurrency) {
    var CusTooltip = {};
    var cusDataLabels = {};
    if(isCurrency == true)
    {
            CusTooltip = {
                y: {
                    formatter: function (value) {
                        return "$" + value.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                 }
                 };
                 cusDataLabels = {
                            enabled: true,
                            offsetY: 1,
                            formatter: function (val) {
                                    return "$" + val.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                },
                            background: {
                                enabled: true,
                                padding: 4,
                                borderRadius: 4,
                                borderWidth: 0,
                                opacity: 0.9,
                                dropShadow: { enabled: false }
                            }
                        };

    }
    else
    {
             CusTooltip = {
                        y: {
                            formatter: function (value) {
                                return value.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","); // Rounds to the nearest integer

                            }
                         }
                         };

                          cusDataLabels = {
                            enabled: true,
                            offsetY: 1,
                            formatter: function (val) {
                                    return val.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","); // Rounds to the nearest integer
                                },
                            background: {
                                enabled: true,
                                padding: 4,
                                borderRadius: 4,
                                borderWidth: 0,
                                opacity: 0.9,
                                dropShadow: { enabled: false }
                            }
                        };
     }

    var options = {
        chart: {
            type: 'line',
            id: id,
            toolbar: { show: false }
        },
        series: series,
        xaxis: {
            categories: categories,
            title: {
                text: xTitle
            }
        },
        yaxis: {
            title: {
                text: yTitle
            },
        },
        dataLabels: cusDataLabels,
        stroke: { curve: 'smooth', width: 3 },
        grid: {
            row: {
                colors: ['#f3f3f3', 'transparent'],
                opacity: 0.5
            }
        },
        markers: { size: 4 },
        legend: {
            show: true,
            position: 'top',
            offsetY: 0,
            horizontalAlign: 'right',
            floating: true
        },
        tooltip : CusTooltip
    }

    var chart = new ApexCharts(document.querySelector("#" + id), options);

    chart.render();
}

function initPieChart2(id, name, data, categories, isCurrency) {
    let plotOptions = {};

    if (isCurrency == true) {
        plotOptions = {
            pie: {
                donut: {
                    labels: {
                        show: true,
                        total: {
                            showAlways: true,
                            show: true,
                            formatter: function (w) {
                                const total = w.globals.seriesTotals.reduce((a, b) => a + b, 0);
                                return "$" + total.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                            }
                        }
                    }
                }
            }
        };
    }
    else
    {
        plotOptions = {
            pie: {
                donut: {
                    labels: {
                        show: true,
                            total: {
                            showAlways: true,
                                show: true,
                                    formatter: function (w) {
                                        const total = w.globals.seriesTotals.reduce((a, b) => a + b, 0);
                                        return total.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                    }
                        }
                    }
                }
            }
        }
    }

    var options = {
        series: data,
        chart: {
            id: id,
            width: 400,
            height: 300,
            type: 'donut',
            dropShadow: {
                enabled: true,
                color: '#111',
                top: -1,
                left: 3,
                blur: 3,
                opacity: 0.2
            }
        },
        stroke: {
            width: 0,
        },
       
        plotOptions: plotOptions,

        labels: categories,
        dataLabels: {
            dropShadow: {
                blur: 3,
                opacity: 0.8
            }
        },
        legend: {
            show: false
        },
        fill: {
            type: 'solid'
        },
        colors: [
            '#36a2eb', '#ff9f40', '#ffce56', '#4bc0c0', '#9966ff',
            '#ff9f40', '#4bc0c0', '#36a2eb', '#ff6384', '#ffce56',
            '#9966ff', '#ff9f40', '#4bc0c0', '#36a2eb', '#ff6384',
            '#ffce56', '#9966ff', '#ff9f40', '#4bc0c0', '#36a2eb'
        ],
        tooltip: {
            y: {
                formatter: function (value) {
                    return "$" + value.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                }
            }
        },
        states: {
            hover: {
                filter: 'none'
            }
        },
        theme: {
            palette: 'palette2'
        },
        title: {
            text: name
        },
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    show: false
                }
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#" + id), options);

    chart.render();
}

function initPieChart(id, name, data, categories, isCurrency, showLegend) {
    let plotOptions = {};
    var cusToolTip = {};

    if (isCurrency == true) {
        plotOptions = {
            pie: {
                donut: {
                    labels: {
                        show: true,
                        total: {
                            showAlways: true,
                            show: true,
                            formatter: function (w) {
                                const total = w.globals.seriesTotals.reduce((a, b) => a + b, 0);
                                return "$" + total.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                            }
                        }
                    }
                }
            }
        };
        cusToolTip = {
            y: {
                formatter: function (value) {
                    return "$" + value.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                }
            }
        };
    }
    else {
        plotOptions = {
            pie: {
                donut: {
                    labels: {
                        show: true,
                        total: {
                            showAlways: true,
                            show: true,
                            formatter: function (w) {
                                const total = w.globals.seriesTotals.reduce((a, b) => a + b, 0);
                                return total.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                            }
                        }
                    }
                }
            }
        };
        cusToolTip = {
            y: {
                formatter: function (value) {
                    return value.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                }
            }
        };
    }
    var cuslegend = false; 
    if(showLegend == true)
    {
         cuslegend = true;
    }

    var options = {
        series: data,
        chart: {
            id: id,
            width: 400,
            height: 300,
            type: 'donut',
            dropShadow: {
                enabled: true,
                color: '#111',
                top: -1,
                left: 3,
                blur: 3,
                opacity: 0.2
            }
        },
        stroke: {
            width: 0,
        },

        plotOptions: plotOptions,

        labels: categories,
        dataLabels: {
            dropShadow: {
                blur: 3,
                opacity: 0.8
            }
        },
        legend: {
            show: cuslegend
        },
        fill: {
            type: 'solid'
        },
        colors: [
            '#36a2eb', '#ff9f40', '#ffce56', '#4bc0c0', '#9966ff',
            '#ff9f40', '#4bc0c0', '#36a2eb', '#ff6384', '#ffce56',
            '#9966ff', '#ff9f40', '#4bc0c0', '#36a2eb', '#ff6384',
            '#ffce56', '#9966ff', '#ff9f40', '#4bc0c0', '#36a2eb'
        ],
        tooltip: cusToolTip,
        states: {
            hover: {
                filter: 'none'
            }
        },
        theme: {
            palette: 'palette2'
        },
        title: {
            text: name
        },
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    show: cuslegend
                }
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#" + id), options);

    chart.render();
}

function capitalize(str) {
    if (typeof str !== 'string') return '';
    return str.charAt(0).toUpperCase() + str.slice(1);
}

function downloadCSV(data, filename = 'File.csv') {
    if (!data.length) return;

    const titleKeysCap = Object.keys(data[0]).map(capitalize);
    const titleKeys = Object.keys(data[0]);

    const refinedData = [titleKeysCap, ...data.map(item => titleKeys.map(key => `"${item[key]}"`))];

    let csvContent = refinedData.map(row => row.join(',')).join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const objUrl = URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = objUrl;
    link.download = filename;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(objUrl);
}


function toDecimal2(val) {
    return Number.parseFloat(val).toFixed(2);
}
function toCurency(amount, decimals) {
    if (!amount) {
        return "$0";
    }
    amount = decimals !== undefined ? parseFloat(amount).toFixed(decimals) : Math.round(amount).toString();
    x = amount.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return "$" + x1 + x2;
}
function tonumberwithcoma(val) {
    return String(val).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function formatAsPercentage(value) {
    return `${value.toFixed(2)}%`;
}