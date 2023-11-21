using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoWebAppApi.Interface
{
    public interface IAzureService
    {
        string GenerateSasToken(string containerName, string blobName);
    }
}