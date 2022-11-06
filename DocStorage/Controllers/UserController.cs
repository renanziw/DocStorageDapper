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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Authenticate(AuthenticateUserRequest model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public ServiceResponse<IEnumerable<User>> Get()
        {
            return _userService.GetAll();
        }

        [Authorize(Role.Admin)]
        [HttpGet("{id}")]
        public ServiceResponse<User> Get(int id)
        {
            return _userService.Get(id);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public ServiceResponse Post(User user)
        {
            return _userService.Add(user);
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public ServiceResponse Delete(int id)
        {
            return _userService.Delete(id);
        }
    }
}
