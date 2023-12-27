using BusinessLayer.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class FileController : Controller
    {
        private readonly IStorageService _storageService;
        public FileController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [AuthorizePermission("file.download")]
        [Route("File/{id}/{name}")]
        public async Task<ActionResult> FileView(string id, string name)
        {
            var fileStream = await _storageService.Get(id);
            Response.AppendHeader("Content-Disposition", "inline; filename=" + name);

            string mimeType = System.Net.Mime.MediaTypeNames.Application.Octet; // default MIME type
            var extension = Path.GetExtension(name).ToLowerInvariant();
            switch (extension)
            {
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimeType = "image/jpeg";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".gif":
                    mimeType = "image/gif";
                    break;
                case ".doc":
                case ".docx":
                    mimeType = "application/vnd.ms-word";
                    break;
                case ".xls":
                case ".xlsx":
                    mimeType = "application/vnd.ms-excel";
                    break;
                case ".txt":
                    mimeType = "text/plain";
                    break;
                case ".html":
                case ".htm":
                    mimeType = "text/html";
                    break;
                    // Add more cases as needed
            }

            return File(fileStream, mimeType);
        }
    }
}