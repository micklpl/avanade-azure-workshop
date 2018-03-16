using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class BinaryFilesRepository
    {
        private CloudBlobClient GetBlobStorageClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["storageConnectionString"]);
            return storageAccount.CreateCloudBlobClient();
        }

        public bool AnyFileExists(string containerName, string fileName)
        {
            var cloudBlobClient = GetBlobStorageClient();
            var container = $"{containerName}-{fileName}".ToLower();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            return cloudBlobContainer.Exists();
        }

        public void SaveBlob(string containerName, string fileName, byte[] bytes)
        {
            var cloudBlobClient = GetBlobStorageClient();
            var container = $"{containerName}-{fileName}".ToLower();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            cloudBlobContainer.CreateIfNotExists();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            cloudBlobContainer.SetPermissions(permissions);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + ".png");

            using (var stream = new MemoryStream(bytes, writable: false))
            {
                cloudBlockBlob.UploadFromStream(stream);
            }            
        }

        public List<string> GetBlobUrls(string containerName, string fileName)
        {
            var cloudBlobClient = GetBlobStorageClient();
            var container = $"{containerName}-{fileName}".ToLower();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            return cloudBlobContainer.ListBlobs().Select(b => b.Uri.ToString()).ToList();
        }
    }
}