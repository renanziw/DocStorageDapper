using DocStorage.Api.Authorization;
using DocStorage.Model;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using Microsoft.AspNetCore.Mvc;

namespace DocStorage.Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class DocumentAccessController : ControllerBase
    {
        private readonly IDocumentAccessService _documentAccessService;

        public DocumentAccessController(IDocumentAccessService documentAccessService)
        {
            _documentAccessService = documentAccessService;
        }

        [HttpGet]
        [Authorize(Role.Admin)]
        public ServiceResponse<IEnumerable<DocumentAccess>> Get()
        {
            return _documentAccessService.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Role.Admin)]
        public ServiceResponse<DocumentAccess> Get(int id)
        {
            return _documentAccessService.Get(id);
        }

        [HttpPost]
        [Authorize(Role.Admin)]
        public ServiceResponse Post(DocumentAccessCreateModel documentAccess)
        {
            return _documentAccessService.Add(documentAccess);
        }

        [HttpDelete("{id}")]
        [Authorize(Role.Admin)]
        public ServiceResponse Delete(int id)
        {
            return _documentAccessService.Delete(id);
        }
    }
}
