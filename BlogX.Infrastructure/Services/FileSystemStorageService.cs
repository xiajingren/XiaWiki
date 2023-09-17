using BlogX.Core.Interfaces;
using BlogX.Infrastructure.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Infrastructure.Services
{
    internal class FileSystemStorageService : IBlobStorageService
    {
        private readonly AppConfig _appConfig;

        public FileSystemStorageService(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public async Task<Stream> GetAsync(string bolbName)
        {
            var path = Path.Combine(_appConfig.BlobPath, bolbName);

            //todo: is null
            // if (!File.Exists(path))
            //     return null;

            var stream = File.OpenRead(path);

            return await Task.FromResult(stream);
        }

        public async Task<bool> PutAsync(string bolbName, Stream stream)
        {
            var path = Path.Combine(_appConfig.BlobPath, bolbName);

            using var fileStream = File.OpenWrite(path);
            await stream.CopyToAsync(fileStream);

            return true;
        }
    }
}
