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
    public class UserGroupService : IUserGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUserGroupRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public UserGroupService(IMapper mapper, IUserGroupRepository repo, IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _repo = repo;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public ServiceResponse<IEnumerable<Model.UserGroup>> GetAll()
        {
            return _repo.GetAll().Select(_mapper.Map<Model.UserGroup>).ToList();
        }

        public ServiceResponse<Model.UserGroup> Get(int id)
        {
            var response = new ServiceResponse<Model.UserGroup>();
            var currentUserGroup = _repo.Get(id);

            if (currentUserGroup == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.RelationBetweenUserAndGroupNotFound));

                return response;
            }

            return _mapper.Map<Model.UserGroup>(currentUserGroup);
        }

        public ServiceResponse Delete(int id)
        {
            var response = new ServiceResponse();
            var isDeleted = _repo.Delete(id);

            if (!isDeleted)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.RelationBetweenUserAndGroupNotFound));

                return response;
            }

            return response;
        }

        public ServiceResponse Add(Model.UserGroupCreateModel userGroup)
        {
            userGroup.ArgsNotNull(nameof(userGroup));

            var result = new ServiceResponse();
            var user = _userRepository.Get(userGroup.UserId);
            var group = _groupRepository.Get(userGroup.GroupId);
            
            if(user == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.UserNotFound));
            }

            if (group == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.GroupNotFound));
            }

            if (!result.Success) return result;

            var isAdded = _repo.Add(userGroup);

            if (!isAdded)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.CouldNotAddUserToGroup));
            }

            return result;
        }
    }
}