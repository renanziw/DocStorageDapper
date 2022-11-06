using AutoMapper;
using DocStorage.Domain;
using DocStorage.Model;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;

namespace DocStorage.Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GroupService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ServiceResponse<IEnumerable<Model.Group>> GetAll()
        {
            return _context.Groups.Select(_mapper.Map<Model.Group>).ToList();
        }

        public ServiceResponse<Model.Group> Get(int id)
        {
            var response = new ServiceResponse<Model.Group>();
            var currentGroup = _context.Groups.Where(p => p.GroupId == id).FirstOrDefault();

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
            var currentGroup = _context.Groups.Where(p => p.GroupId == id).FirstOrDefault();

            if (currentGroup == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.GroupNotFound));

                return response;
            }
            
            _context.Groups.Remove(currentGroup);
            _context.SaveChanges();

            return response;
        }

        public ServiceResponse Add(Model.Group group)
        {
            group.ArgsNotNull(nameof(group));

            var result = new ServiceResponse();
            var entity = _mapper.Map<Domain.Group>(group);

            result.AddObjectValidation(entity);
            if (!result.Success) return result;

            _context.Groups.Add(entity);
            _context.SaveChanges();

            return result;
        }
    }
}