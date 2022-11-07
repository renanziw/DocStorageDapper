using DocStorage.Model;

namespace DocStorage.Repository.Contracts
{
    public interface IUserGroupRepository : IBaseRepository<Domain.UserGroup>
    {
        public bool Add(Model.UserGroupCreateModel entity);
        public List<Domain.UserGroup> GetAllByUserId(int id);
    }
}
