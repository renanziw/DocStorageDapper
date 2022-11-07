using AutoMapper;
using DocStorage.Domain;
using DocStorage.Repository.Contracts;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DocStorage.Service.Services
{
    public class DocumentAccessService : IDocumentAccessService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentAccessRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IDocumentRepository _documentRepository;

        public DocumentAccessService(IMapper mapper, IDocumentAccessRepository repo, IUserRepository userRepository, IGroupRepository groupRepository, IDocumentRepository documentRepository)
        {
            _mapper = mapper;
            _repo = repo;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _documentRepository = documentRepository;
        }

        public ServiceResponse<IEnumerable<Model.DocumentAccess>> GetAll()
        {
            return _repo.GetAll().Select(_mapper.Map<Model.DocumentAccess>).ToList();
        }

        public ServiceResponse<Model.DocumentAccess> Get(int id)
        {
            var response = new ServiceResponse<Model.DocumentAccess>();
            var documentAccess = _repo.Get(id);

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
            var isDeleted = _repo.Delete(id);

            if (!isDeleted)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.DocumentAccessNotFound));

                return response;
            }

            return response;
        }

        public ServiceResponse Add(Model.DocumentAccessCreateModel documentAccess)
        {
            documentAccess.ArgsNotNull(nameof(documentAccess));

            var result = new ServiceResponse();
            var user = _userRepository.Get(documentAccess.UserId);
            var group = _groupRepository.Get(documentAccess.GroupId);
            var document = _documentRepository.Get(documentAccess.DocumentId);

            if(document == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.DocumentNotFound));
            }

            if(user == null && group == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.NeedToSpecifyAtLeatOneAccessType));
            }

            if (!result.Success) return result;

            var isAdded = _repo.Add(documentAccess);

            if (!isAdded)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.CouldNotAddDocumentAccess));
            }

            return result;
        }
    }
}