using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Activities;

namespace WebApp.Controllers
{
    public class FileController : Controller
    {
        private readonly IStorageService _storageService;
        public FileController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        // GET: File
        [Route("File/{id}/{name}")]
        public FileResult Index(string id, string name)
        {
            var fileStream = _storageService.Get(id);
            return File(fileStream, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}