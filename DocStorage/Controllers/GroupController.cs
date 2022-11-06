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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [Authorize(Role.Admin)]
        public ServiceResponse<IEnumerable<Group>> Get()
        {
            return _groupService.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Role.Admin)]
        public ServiceResponse<Group> Get(int id)
        {
            return _groupService.Get(id);
        }

        [HttpPost]
        [Authorize(Role.Admin)]
        public ServiceResponse Post(Group group)
        {
            return _groupService.Add(group);
        }

        [HttpDelete("{id}")]
        [Authorize(Role.Admin)]
        public ServiceResponse Delete(int id)
        {
            return _groupService.Delete(id);
        }
    }
}
