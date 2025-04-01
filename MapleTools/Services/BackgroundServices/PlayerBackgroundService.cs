using MapleTools.Abstraction;
using MapleTools.Models.Api;
using MapleTools.Models.BackgroundService;
using MapleTools.Util;
using System.Text.Json;


namespace MapleTools.Services.BackgroundServices
{
    public class PlayerBackgroundService : BackgroundService
    {
        private readonly ILogger<PlayerBackgroundService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private string _lastExecutionFilePath;
        private string _playerFilePath;
        private readonly TimeSpan _checkInterval;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileAccessor _fileAccessor;

        public PlayerBackgroundService(
            ILogger<PlayerBackgroundService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IFileAccessor fileAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _lastExecutionFilePath = Path.Combine(
                _webHostEnvironment.ContentRootPath,
                _configuration["PlayerDataService:LastRun"] ?? "Data\\BackgroundServices\\LastRun");
            _playerFilePath = Path.Combine(
               _webHostEnvironment.ContentRootPath,
               _configuration["PlayerDataService:LastRun"] ?? "Data\\BackgroundServices\\Player");

            _checkInterval = TimeSpan.FromHours(
                _configuration.GetValue<double>("PlayerDataService:RunningIntervalMinutes", 10));
            _fileAccessor = fileAccessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PlayerDataBackgroundService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogDebug("Checking if player data needs to be processed...");

                    // Check if we need to execute the main logic
                    if (await ShouldExecuteDataProcessing())
                    {
                        _logger.LogInformation("Starting player data processing...");
                        await ProcessPlayerDataAsync();

                        // Update the last execution time
                        UpdateLastExecutionTime();
                        _logger.LogInformation("Player data processing completed successfully.");
                    }
                    else
                    {
                        _logger.LogDebug("Data processing not required at this time.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing player data.");
                }

                // Wait for the next check interval
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task<bool> ShouldExecuteDataProcessing()
        {
            // If the file doesn't exist, we should execute
            if (!File.Exists(_lastExecutionFilePath))
            {
                _logger.LogInformation("Last execution file not found. Will execute data processing.");
                return true;
            }

            try
            {
                // Read the last execution time from the file
                var lastExecute = await _fileAccessor.JsonFileReader<LastExecute>(_lastExecutionFilePath,"",-1);

                // Check if it's been more than 24 hours since the last execution
                TimeSpan timeSinceLastExecution = DateTime.UtcNow - lastExecute.PlayerBackgroundService;
                bool shouldExecute = timeSinceLastExecution.TotalHours >= 24;

                _logger.LogInformation(
                    "Hours since last execution: {Hours}. Should execute: {ShouldExecute}",
                    timeSinceLastExecution.TotalHours,
                    shouldExecute);

                return shouldExecute;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking last execution time. Will execute data processing.");
                return true;
            }
        }

        private async Task UpdateLastExecutionTime()
        {
            try
            {
                var lastExecute =await _fileAccessor.JsonFileReader<LastExecute>(_lastExecutionFilePath, "", -1);
                lastExecute.PlayerBackgroundService = DateTime.UtcNow;
                await _fileAccessor.JsonFileWriter<LastExecute>(_lastExecutionFilePath, "", SaveMode.Simple, lastExecute);
                _logger.LogDebug("Updated last execution time.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update last execution time.");
            }
        }

        private async Task ProcessPlayerDataAsync()
        {
            // Get configuration values
            string baseUrl = _configuration["PlayerDataService:BaseUrl"];
            string id = _configuration["PlayerDataService:Id"]??"legendary";
            int maxPages = _configuration.GetValue<int>("PlayerDataService:MaxPages", 10);

            // Create HTTP client
            using var httpClient = _httpClientFactory.CreateClient("PlayerDataApi");
            List<Player> result = new List<Player>();
            // Process multiple pages
            for (int pageIndex = 1; pageIndex <= maxPages; pageIndex = pageIndex+10)
            {
                try
                {
                    // Construct the API URL
                    string apiUrl = $"{baseUrl}/na/?id={id}&page_index={pageIndex}";
                    _logger.LogInformation("Fetching data from: {Url}", apiUrl);

                    // Make the API request
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    // Parse the response
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Player[] players = JsonSerializer.Deserialize<Player[]>(jsonResponse);

                    // Check if we received data
                    if (players == null || players.Length == 0)
                    {
                        _logger.LogInformation("No more player data available after page {Page}.", pageIndex);
                        break;
                    }
                    result.AddRange(players);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing page {Page}", pageIndex);
                    // Continue with the next page even if one fails
                }
            }
            await _fileAccessor.JsonFileWriter<List<Player>>(_playerFilePath, "", SaveMode.VersionNoLanguage, result);
        }
    }
}
