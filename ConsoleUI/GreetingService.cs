using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// Console app which implements: Dependency Injection, Logging/Serilog, Settings.

namespace ConsoleUI
{
    
    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _log;
        private readonly IConfiguration _config;

        public GreetingService(ILogger<GreetingService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public void Run()
        {
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                // Create a log record (note: this is not string interpolation - the value for i is also stored separately).
                _log.LogInformation("Run number {runNumber}", i);
            }
        }
    }
}
