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
    public class UserGroupController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;

        public UserGroupController(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        [HttpGet]
        [Authorize(Role.Admin)]
        public ServiceResponse<IEnumerable<UserGroup>> Get()
        {
            return _userGroupService.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Role.Admin)]
        public ServiceResponse<UserGroup> Get(int id)
        {
            return _userGroupService.Get(id);
        }

        [HttpPost]
        [Authorize(Role.Admin)]
        public ServiceResponse Post(UserGroupCreateModel userGroup)
        {
            return _userGroupService.Add(userGroup);
        }

        [HttpDelete("{id}")]
        [Authorize(Role.Admin)]
        public ServiceResponse Delete(int id)
        {
            return _userGroupService.Delete(id);
        }
    }
}
