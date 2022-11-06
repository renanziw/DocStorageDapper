using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using DocStorage.Domain;
using DocStorage.Model;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;

namespace DocStorage.Service.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IJwtUtils jwtUtils, IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public ServiceResponse<IEnumerable<Model.User>> GetAll()
        {
            return _context.Users.Select(_mapper.Map<Model.User>).ToList();
        }

        public ServiceResponse<Model.User> Get(int id)
        {
            var response = new ServiceResponse<Model.User>();
            var currentUser = _context.Users.Where(p => p.UserId == id).FirstOrDefault();

            if (currentUser == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.UserNotFound));

                return response;
            }

            return _mapper.Map<Model.User>(currentUser);
        }

        public ServiceResponse Delete(int id)
        {
            var response = new ServiceResponse();
            var currentUser = _context.Users.Where(p => p.UserId == id).FirstOrDefault();

            if (currentUser == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.UserNotFound));

                return response;
            }
            
            _context.Users.Remove(currentUser);
            _context.SaveChanges();

            return response;
        }

        public ServiceResponse Add(Model.User user)
        {
            user.ArgsNotNull(nameof(user));

            var result = new ServiceResponse();
            var entity = _mapper.Map<Domain.User>(user);

            result.AddObjectValidation(entity);
            if (!result.Success) return result;

            entity.PasswordHash = BCryptNet.HashPassword(user.Password);

            _context.Users.Add(entity);
            _context.SaveChanges();

            return result;
        }

        public ServiceResponse<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest model)
        {
            var response = new ServiceResponse<AuthenticateUserResponse>();
            var user = _context.Users.FirstOrDefault(x => x.UserName == model.UserName);

            if (user == null)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.UserNotFound));

                return response;
            }

            if (!BCryptNet.Verify(model.Password, user.PasswordHash))
            {
                response.Errors.Add(new ServiceError(ErrorTypes.UserOrPasswordIncorrect));

                return response;
            }

            var userModel = _mapper.Map<Model.User>(user);
            var jwtToken = _jwtUtils.GenerateJwtToken(userModel);

            return new AuthenticateUserResponse(userModel, jwtToken);
        }
    }
}