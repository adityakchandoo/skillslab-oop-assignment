using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IStorageService
    {
        Task Put(Stream stream, string systemFileName);
        Task<Stream> Get(string systemFileName);
    }
}
