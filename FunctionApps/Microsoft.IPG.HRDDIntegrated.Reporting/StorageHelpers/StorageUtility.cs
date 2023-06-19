namespace Microsoft.IPG.HRDDIntegrated.Reporting.StorageHelpers
{
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    internal class StorageUtility
    {
        private static readonly CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureStorageConnection"));

        /// <summary>
        /// Upload file into the blob storage, create container if not created yet. the parameter being passed in is the byte array of the file content not the location of the file.
        /// </summary>
        /// <param name="containername"></param>
        /// <param name="filename"></param>
        /// <param name="file"></param>
        /// <param name="pageIndex"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async static Task<bool> UploadFileAsync(string containername, string filename, byte[] file, int pageIndex, ILogger log)
        {
            bool successful = false;
            try
            {
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(containername);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named.
                CloudAppendBlob appendBlob = container.GetAppendBlobReference(filename);

                // Create or replace only if the blob doesn't exist
                if (!appendBlob.ExistsAsync().Result)
                    await appendBlob.CreateOrReplaceAsync();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Append the blob with contents from a local file.
                MemoryStream stream = new MemoryStream(file);
                await appendBlob.AppendFromStreamAsync(stream);

                stopwatch.Stop();
                log.LogInformation($"UploadFileBatch - Time taken for batch - {pageIndex} upload process: {stopwatch.Elapsed.TotalMinutes} Mins");
                successful = true;
            }
            catch (Exception ex)
            {
                //Logging
                log.LogError("UploadFileBatch - " + ex);
            }
            return successful;
        }
    }
}
