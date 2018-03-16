using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class BinaryFilesRepository
    {
        public async Task<bool> FileExists(string containerName, string fileName)
        {
            await Task.Delay(0);
            return false;
        }

        public async Task SaveBlob(string containerName, string fileName, byte[] bytes)
        {
            await Task.Delay(0);
        }

        public async Task<List<string>> GetBlobUrls(string containerName, string fileName)
        {
            await Task.Delay(0);
            return Enumerable.Empty<string>().ToList();
        }
    }
}