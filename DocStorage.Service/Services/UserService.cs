using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using DocStorage.Domain;
using DocStorage.Model;
using DocStorage.Service.Extensions;
using DocStorage.Service.Interfaces;
using DocStorage.Util;
using DocStorage.Util.Extensions;
using DocStorage.Repository.Contracts;

namespace DocStorage.Service.Services
{
    public class UserService : IUserService
    {
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repo;

        public UserService(IJwtUtils jwtUtils, IMapper mapper, IUserRepository repo)
        {
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _repo = repo;
        }

        public ServiceResponse<IEnumerable<Model.User>> GetAll()
        {
            var users = _repo.GetAll();

            return users.Select(_mapper.Map<Model.User>).ToList();
        }

        public ServiceResponse<Model.User> Get(int id)
        {
            var adminHash = BCryptNet.HashPassword("admin");
            var managerHash = BCryptNet.HashPassword("manager");
            var regularHash = BCryptNet.HashPassword("regular");
            var response = new ServiceResponse<Model.User>();
            var currentUser = _repo.Get(id);

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
            var isDeleted = _repo.Delete(id);

            if (isDeleted == false)
            {
                response.Errors.Add(new ServiceError(ErrorTypes.UserNotFound));

                return response;
            }

            return response;
        }

        public ServiceResponse Add(Model.User user)
        {
            user.ArgsNotNull(nameof(user));

            var result = new ServiceResponse();
            var hash = BCryptNet.HashPassword(user.Password);
            user.Password = hash;

            var isAdded = _repo.Add(user);

            if(isAdded == false)
            {
                result.Errors.Add(new ServiceError(ErrorTypes.CouldNotAddUser));
            }

            return result;
        }

        public ServiceResponse<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest model)
        {
            var response = new ServiceResponse<AuthenticateUserResponse>();
            var user = _repo.GetByUsername(model.UserName);

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