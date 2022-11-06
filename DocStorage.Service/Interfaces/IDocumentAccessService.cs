using DocStorage.Model;
using DocStorage.Util;

namespace DocStorage.Service.Interfaces
{
    public interface IDocumentAccessService : IBaseService<DocumentAccessCreateModel>
    {
        public ServiceResponse<IEnumerable<DocumentAccess>> GetAll();
        public ServiceResponse Add(DocumentAccessCreateModel model);
        public ServiceResponse<DocumentAccess> Get(int id);
    }
}
