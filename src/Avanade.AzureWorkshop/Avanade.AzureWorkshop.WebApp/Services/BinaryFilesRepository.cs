using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class BinaryFilesRepository
    {
        public bool AnyFileExists(string containerName, string fileName)
        {
            return false;
        }

        public void SaveBlob(string containerName, string fileName, byte[] bytes)
        {
        }

        public List<string> GetBlobUrls(string containerName, string fileName)
        {
            return Enumerable.Empty<string>().ToList();
        }
    }
}