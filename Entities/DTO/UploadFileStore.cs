using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class UploadFileStore
    {
        public int FileId;
        public string FileName;
        public Stream FileContent;
    }
}
