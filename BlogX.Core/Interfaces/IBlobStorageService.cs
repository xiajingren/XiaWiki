using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Core.Interfaces
{
    public interface IBlobStorageService
    {
        Task<bool> PutAsync(string bolbName, Stream stream);

        Task<Stream> GetAsync(string bolbName);
    }
}
