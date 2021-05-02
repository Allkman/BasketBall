using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Server.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task DeleteFile(string containerName, string fileRoute);
        Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute);
        Task<string> SaveFile(byte[] content, string extension, string containerName);
    }
}
