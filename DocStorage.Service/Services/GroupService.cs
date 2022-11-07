using AutoMapper;
using DocStorage.Domain;
using DocStorage.Model;
using DocStorage.Repository.Contracts;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;

namespace DocStorage.Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _repo;

        public GroupService(IMapper mapper, IGroupRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public ServiceResponse<IEnumerable<Model.Group>> GetAll()
        {
            var groups = _repo.GetAll();

            return groups.Select(_mapper.Map<Model.Group>).ToList();
        }

        public ServiceResponse<Model.Group> Get(int id)
        {
            var response = new ServiceResponse<Model.Group>();
            var currentGroup = _repo.Get(id); ;

            if (currentGroup == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.GroupNotFound));

                return response;
            }

            return _mapper.Map<Model.Group>(currentGroup);
        }

        public ServiceResponse Delete(int id)
        {
            var response = new ServiceResponse();
            var isDeleted = _repo.Delete(id);

            if (!isDeleted)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.GroupNotFound));

                return response;
            }

            return response;
        }

        public ServiceResponse Add(Model.Group group)
        {
            group.ArgsNotNull(nameof(group));

            var result = new ServiceResponse();
            var isAdded = _repo.Add(group);

            if (!isAdded)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.CouldNotAddGroup));

                return result;
            }

            return result;
        }
    }
}