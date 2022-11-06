using DocStorage.Api.Authorization;
using DocStorage.Model;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using Microsoft.AspNetCore.Mvc;

namespace DocStorage.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        const string folderName = "files";
        readonly string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpGet]
        public ServiceResponse<IEnumerable<Document>> Get()
        {
            return _documentService.GetAll();
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPost]
        public ServiceResponse Post(string description, [FromHeader] string category, [FromForm] IFormFile file)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            Document document = null;
            var filename = Guid.NewGuid().ToString();
            var fileExtension = file.FileName.Split('.').Last();
            var filePath = Path.Combine(folderPath, $"{filename}.{fileExtension}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(stream);
                document = new Document(description, category, filename, fileExtension);
            }

            return _documentService.Add(document);
        }

        [Authorize(Role.Regular, Role.Admin, Role.Manager)]
        [HttpGet("{id}")]
        public IActionResult Download(int id)
        {
            var user = (User)HttpContext.Items["User"];
            var response = _documentService.Get(id, user.UserId.Value);

            if (!response.Success)
            {
                return NotFound();
            }

            var document = response.Data;
            var filePath = Path.Combine(folderPath, document.FileName);

            CreateFileIfDoesntExists(filePath);

            return File(System.IO.File.ReadAllBytes(filePath), "application/octet-stream", document.FileName);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpDelete("{id}")]
        public ServiceResponse Delete(int id)
        {
            return _documentService.Delete(id);
        }

        //deal with Unit tests
        private static void CreateFileIfDoesntExists(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(filePath))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
        }
    }
}
