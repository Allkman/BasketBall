using Azure.Storage.Blobs;
using BasketBall.Server.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
namespace BasketBall.Server.Services
{
    public class FileStorageService : IFileStorageService
    {
        private string connectionString;

        public FileStorageService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorageConnection");
        }
        //content - string representation of file Base64String..
        //extension - jpg, jpeg, png...
        //containerName - folder name in azure
        //fileRoute - url of a file
        public async Task<string> EditFile(byte[] content, string extension,
            string containerName, string fileRoute)
        {
            await DeleteFile(fileRoute, containerName);
            return await SaveFile(content, extension, containerName);
        }

        public async Task DeleteFile(string containerName, string fileRoute)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(fileRoute);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }
        public async Task<string> SaveFile(byte[] content, string extension, string containerName)
        {
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            using (var ms = new MemoryStream(content))
            {
                await blob.UploadAsync(ms);
            }
            return blob.Uri.ToString();
        }
    }
}
