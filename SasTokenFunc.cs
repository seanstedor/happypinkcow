using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
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
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage");
            var containerName = Environment.GetEnvironmentVariable("ContainerName");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);

            SharedAccessBlobPolicy accessPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30),
                Permissions = SharedAccessBlobPermissions.Create | SharedAccessBlobPermissions.Write
            };

            var token = container.GetSharedAccessSignature(accessPolicy);

            return new OkObjectResult($"{container.Uri}{token}");
        }
    }
}
