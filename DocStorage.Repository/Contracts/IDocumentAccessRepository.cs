using DocStorage.Model;

namespace DocStorage.Repository.Contracts
{
    public interface IDocumentAccessRepository : IBaseRepository<Domain.DocumentAccess>
    {
        public bool Add(Model.DocumentAccessCreateModel entity);
        public List<Domain.DocumentAccess> GetAllByDocumentId(int id);
    }
}
