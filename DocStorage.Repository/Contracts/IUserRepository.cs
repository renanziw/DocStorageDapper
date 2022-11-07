using DocStorage.Model;

namespace DocStorage.Repository.Contracts
{
    public interface IUserRepository : IBaseRepository<Domain.User>
    {
        public bool Add(Model.User entity);
        public Domain.User GetByUsername(string username);
    }
}
