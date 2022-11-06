using DocStorage.Model;
using DocStorage.Util;

namespace DocStorage.Service.Interfaces
{
    public interface IUserGroupService : IBaseService<UserGroupCreateModel>
    {
        public ServiceResponse<IEnumerable<UserGroup>> GetAll();
        public ServiceResponse Add(UserGroupCreateModel model);
        public ServiceResponse<UserGroup> Get(int id);
    }
}
