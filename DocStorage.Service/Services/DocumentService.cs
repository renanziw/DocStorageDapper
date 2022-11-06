using AutoMapper;
using DocStorage.Domain;
using DocStorage.Model;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DocStorage.Service.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DocumentService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ServiceResponse<IEnumerable<Model.Document>> GetAll()
        {
            return _context.Documents.Select(_mapper.Map<Model.Document>).ToList();
        }

        public ServiceResponse<Model.Document> Get(int documentId, int currentUserId)
        {
            var response = new ServiceResponse<Model.Document>();
            var currentDocument = _context.Documents.Where(p => p.DocumentId == documentId).FirstOrDefault();
            var userGroups = _context.UserGroups.Include(p => p.User).Include(p => p.Group).Where(p => p.User != null && p.User.UserId == currentUserId).ToList();
            var accessList = _context.DocumentAccess.Include(p => p.User).Include(p => p.Group).Include(p => p.Document).Where(p => p.Document.DocumentId == documentId).ToList();

            if (currentDocument == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.DocumentNotFound));
            }

            if ((!accessList.Any(p => p.User != null && p.User.UserId == currentUserId)) && !accessList.Any(p => p.Group != null && userGroups.Any(q => q.Group != null && q.Group.GroupId == p.Group.GroupId)))
            {
                response.Errors.Add(new ServiceError(ErrorTypes.Unauthorized));
            }

            if (!response.Success) return response;

            return _mapper.Map<Model.Document>(currentDocument);
        }

        public ServiceResponse Delete(int id)
        {
            var response = new ServiceResponse();
            var currentDocument = _context.Documents.Where(p => p.DocumentId == id).FirstOrDefault();

            if (currentDocument == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.DocumentNotFound));

                return response;
            }
            
            _context.Documents.Remove(currentDocument);
            _context.SaveChanges();

            return response;
        }

        public ServiceResponse Add(Model.Document document)
        {
            document.ArgsNotNull(nameof(document));

            var result = new ServiceResponse();
            var entity = _mapper.Map<Domain.Document>(document);

            result.AddObjectValidation(entity);
            if (!result.Success) return result;

            _context.Documents.Add(entity);
            _context.SaveChanges();

            return result;
        }
    }
}