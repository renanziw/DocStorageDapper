using DocStorage.Model;
using DocStorage.Util;

namespace DocStorage.Service.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        public ServiceResponse<IEnumerable<User>> GetAll();
        public ServiceResponse Add(User model);
        public ServiceResponse<User> Get(int id);
        public ServiceResponse<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest model);

    }
}
