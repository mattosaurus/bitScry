using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bitScry.Function
{
    public static class SasToken
    {
        [FunctionName("SasToken")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            log.Info("SasToken recieved a request.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data.container == null)
            {
                log.Error("No container value specified.");
                return new BadRequestObjectResult("Specify value for 'container'");
            }

            var permissions = SharedAccessBlobPermissions.Read; // default to read permissions
            bool success = Enum.TryParse(data.permissions.ToString(), out permissions);

            if (!success)
            {
                log.Error("Invalid permission values specified.");
                return new BadRequestObjectResult("Invalid value for 'permissions'");
            }

            var storageAccount = CloudStorageAccount.Parse(config.GetConnectionString("AzureStorageConnection"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(data.container.ToString());

            var sasToken =
                data.blobName != null ?
                    GetBlobSasToken(container, data.blobName.ToString(), permissions) :
                    GetContainerSasToken(container, permissions);

            log.Info("SAS token retrieved successfully.");
            return new OkObjectResult(new
            {
                token = sasToken,
                uri = container.Uri + sasToken
            });
        }

        public static string GetBlobSasToken(CloudBlobContainer container, string blobName, SharedAccessBlobPermissions permissions, string policyName = null)
        {
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                var adHocSas = CreateAdHocSasPolicy(permissions);

                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = blob.GetSharedAccessSignature(adHocSas);
            }
            else
            {
                // Generate the shared access signature on the blob. In this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);
            }

            return sasBlobToken;
        }

        public static string GetContainerSasToken(CloudBlobContainer container, SharedAccessBlobPermissions permissions, string storedPolicyName = null)
        {
            string sasContainerToken;

            // If no stored policy is specified, create a new access policy and define its constraints.
            if (storedPolicyName == null)
            {
                var adHocSas = CreateAdHocSasPolicy(permissions);

                // Generate the shared access signature on the container, setting the constraints directly on the signature.
                sasContainerToken = container.GetSharedAccessSignature(adHocSas, null);
            }
            else
            {
                // Generate the shared access signature on the container. In this case, all of the constraints for the
                // shared access signature are specified on the stored access policy, which is provided by name.
                // It is also possible to specify some constraints on an ad-hoc SAS and others on the stored access policy.
                // However, a constraint must be specified on one or the other; it cannot be specified on both.
                sasContainerToken = container.GetSharedAccessSignature(null, storedPolicyName);
            }

            return sasContainerToken;
        }

        private static SharedAccessBlobPolicy CreateAdHocSasPolicy(SharedAccessBlobPermissions permissions)
        {
            // Create a new access policy and define its constraints.
            // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad-hoc SAS, and 
            // to construct a shared access policy that is saved to the container's shared access policies. 

            return new SharedAccessBlobPolicy()
            {
                // Set start time to five minutes before now to avoid clock skew.
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                Permissions = permissions
            };
        }
    }
}
