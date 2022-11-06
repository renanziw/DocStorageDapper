using DocStorage.Model;
using DocStorage.Util;

namespace DocStorage.Service.Interfaces
{
    public interface IDocumentService : IBaseService<Document>
    {
        public ServiceResponse<IEnumerable<Document>> GetAll();
        public ServiceResponse Add(Document model);
        public ServiceResponse<Document> Get(int documentId, int currentUserId);
    }
}
