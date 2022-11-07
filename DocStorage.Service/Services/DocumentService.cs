using AutoMapper;
using DocStorage.Domain;
using DocStorage.Model;
using DocStorage.Repository.Contracts;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DocStorage.Service.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _repo;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IDocumentAccessRepository _documentAccessRepository;


        public DocumentService(IMapper mapper, IDocumentRepository repo, IUserGroupRepository userGroupRepository, IDocumentAccessRepository documentAccessRepository)
        {
            _mapper = mapper;
            _repo = repo;
            _userGroupRepository = userGroupRepository;
            _documentAccessRepository = documentAccessRepository;
        }

        public ServiceResponse<IEnumerable<Model.Document>> GetAll()
        {
            return _repo.GetAll().Select(_mapper.Map<Model.Document>).ToList();
        }

        public ServiceResponse<Model.Document> Get(int documentId, int currentUserId)
        {
            var response = new ServiceResponse<Model.Document>();
            var currentDocument = _repo.Get(documentId);
            var userGroups = _userGroupRepository.GetAllByUserId(currentUserId).ToList();
            var accessList = _documentAccessRepository.GetAllByDocumentId(documentId);

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
            var isDeleted = _repo.Delete(id);

            if (!isDeleted)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.DocumentNotFound));

                return response;
            }

            return response;
        }

        public ServiceResponse Add(Model.Document document)
        {
            document.ArgsNotNull(nameof(document));

            var result = new ServiceResponse();
            var isAdded = _repo.Add(document);

            if (!isAdded)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.DocumentCouldNotBeAdded));

                return result;
            }

            return result;
        }
    }
}