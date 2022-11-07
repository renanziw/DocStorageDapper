using DocStorage.Model;

namespace DocStorage.Repository.Contracts
{
    public interface IDocumentRepository : IBaseRepository<Domain.Document>
    {
        public bool Add(Model.Document entity);
    }
}
