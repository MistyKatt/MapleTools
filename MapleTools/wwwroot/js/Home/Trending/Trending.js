<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            const charts = {};
            
            @foreach (var level in Model.LevelTabs)
            {
                <text>
                const ctx@(level.Level) = document.getElementById('chart-@level.Level');
                charts[@level.Level] = new Chart(ctx@(level.Level), {
                    type: 'bar',
                    data: {
                        labels: @Json.Serialize(level.JobData.Select(j => j.JobName)),
                        datasets: [{
                            label: 'Player Count',
                            data: @Json.Serialize(level.JobData.Select(j => j.PlayerCount)),
                            backgroundColor: createGradient(ctx@(level.Level), '#4a90e2', '#2c6cd4'),
                            borderWidth: 0,
                            borderRadius: 8,
                            hoverBackgroundColor: '#1a4a8d'
                        }]
                    },
                    options: getChartOptions()
                });
                </text>
            }

            function createGradient(ctx, color1, color2) {
                const gradient = ctx.createLinearGradient(0, 0, 0, 400);
                gradient.addColorStop(0, color1);
                gradient.addColorStop(1, color2);
                return gradient;
            }

            function getChartOptions() {
                return {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            grid: {
                                color: 'rgba(0,0,0,0.05)'
                            },
                            ticks: {
                                stepSize: 50
                            }
                        },
                        x: {
                            grid: {
                                display: false
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            display: false
                        },
                        tooltip: {
                            backgroundColor: '#1a233b',
                            titleFont: { size: 14 },
                            bodyFont: { size: 14 },
                            padding: 12,
                            cornerRadius: 8
                        }
                    },
                    animation: {
                        duration: 800,
                        easing: 'easeOutQuart'
                    }
                };
            }
        });
    </script>