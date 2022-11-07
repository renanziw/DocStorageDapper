using DocStorage.Model;

namespace DocStorage.Repository.Contracts
{
    public interface IGroupRepository : IBaseRepository<Domain.Group>
    {
        public bool Add(Model.Group entity);
    }
}
