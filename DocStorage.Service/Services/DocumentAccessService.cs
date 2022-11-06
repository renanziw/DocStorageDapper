using AutoMapper;
using DocStorage.Domain;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DocStorage.Service.Services
{
    public class DocumentAccessService : IDocumentAccessService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DocumentAccessService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ServiceResponse<IEnumerable<Model.DocumentAccess>> GetAll()
        {
            return _context.DocumentAccess.Include(p => p.Group).Include(p => p.User)
                .Select(_mapper.Map<Model.DocumentAccess>).ToList();
        }

        public ServiceResponse<Model.DocumentAccess> Get(int id)
        {
            var response = new ServiceResponse<Model.DocumentAccess>();
            var documentAccess = _context.DocumentAccess.Where(p => p.DocumentAccessId == id).FirstOrDefault();

            if (documentAccess == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.DocumentAccessNotFound));

                return response;
            }

            return _mapper.Map<Model.DocumentAccess>(documentAccess);
        }

        public ServiceResponse Delete(int id)
        {
            var response = new ServiceResponse();
            var documentAccess = _context.DocumentAccess.Where(p => p.DocumentAccessId == id).FirstOrDefault();

            if (documentAccess == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.DocumentAccessNotFound));

                return response;
            }
            
            _context.DocumentAccess.Remove(documentAccess);
            _context.SaveChanges();

            return response;
        }

        public ServiceResponse Add(Model.DocumentAccessCreateModel documentAccess)
        {
            documentAccess.ArgsNotNull(nameof(documentAccess));

            var result = new ServiceResponse();
            var user = _context.Users.FirstOrDefault(p => p.UserId == documentAccess.UserId);
            var group = _context.Groups.FirstOrDefault(p => p.GroupId == documentAccess.GroupId);
            var document = _context.Documents.FirstOrDefault(p => p.DocumentId == documentAccess.DocumentId);

            if(document == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.DocumentNotFound));
            }

            if(user == null && group == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.NeedToSpecifyAtLeatOneAccessType));
            }

            if (!result.Success) return result;

            var entity = new DocumentAccess(document, group, user);

            _context.DocumentAccess.Add(entity);
            _context.SaveChanges();

            return result;
        }
    }
}