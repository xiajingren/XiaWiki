using BlogX.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Infrastructure.Services
{
    internal class BlobStorageService : IBlobStorageService
    {
        public async Task<Stream> GetAsync(string bolbName)
        {
            var stream = File.OpenRead(bolbName);

            return await Task.FromResult(stream);
        }

        public async Task<bool> PutAsync(string bolbName, Stream stream)
        {
            using var fileStream = File.OpenWrite(bolbName);

            await stream.CopyToAsync(fileStream);

            return true;
        }
    }
}
