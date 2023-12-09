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
        void Put(Stream stream, string systemFileName);
        Stream Get(string systemFileName);
    }
}
