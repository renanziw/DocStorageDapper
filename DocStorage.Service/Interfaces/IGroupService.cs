using DocStorage.Model;
using DocStorage.Util;

namespace DocStorage.Service.Interfaces
{
    public interface IGroupService : IBaseService<Group>
    {
        public ServiceResponse<IEnumerable<Group>> GetAll();
        public ServiceResponse Add(Group model);
        public ServiceResponse<Group> Get(int id);
    }
}
