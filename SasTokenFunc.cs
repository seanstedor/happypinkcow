using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace sowa.dor
{
    public class SasTokenFunc
    {
        private readonly ILogger<SasTokenFunc> _logger;

        public SasTokenFunc(ILogger<SasTokenFunc> logger)
        {
            _logger = logger;
        }

        [Function("SasTokenFunc")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage") ?? "hello world";

            return new OkObjectResult(connectionString);
        }
    }
}
