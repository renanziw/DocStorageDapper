using AutoMapper;
using DocStorage.Domain;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DocStorage.Service.Services
{
    public class UserGroupService : IUserGroupService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserGroupService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ServiceResponse<IEnumerable<Model.UserGroup>> GetAll()
        {
            return _context.UserGroups.Include(p => p.Group).Select(_mapper.Map<Model.UserGroup>).ToList();
        }

        public ServiceResponse<Model.UserGroup> Get(int id)
        {
            var response = new ServiceResponse<Model.UserGroup>();
            var currentUserGroup = _context.UserGroups.Where(p => p.UserGroupId == id).FirstOrDefault();

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
            var currentUserGroup = _context.UserGroups.Where(p => p.UserGroupId == id).FirstOrDefault();

            if (currentUserGroup == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.RelationBetweenUserAndGroupNotFound));

                return response;
            }
            
            _context.UserGroups.Remove(currentUserGroup);
            _context.SaveChanges();

            return response;
        }

        public ServiceResponse Add(Model.UserGroupCreateModel userGroup)
        {
            userGroup.ArgsNotNull(nameof(userGroup));

            var result = new ServiceResponse();
            var user = _context.Users.FirstOrDefault(p => p.UserId == userGroup.UserId);
            var group = _context.Groups.FirstOrDefault(p => p.GroupId == userGroup.GroupId);
            
            if(user == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.UserNotFound));
            }

            if (group == null)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.GroupNotFound));
            }

            if (!result.Success) return result;

            var entity = new UserGroup(user, group);

            _context.UserGroups.Add(entity);
            _context.SaveChanges();

            return result;
        }
    }
}