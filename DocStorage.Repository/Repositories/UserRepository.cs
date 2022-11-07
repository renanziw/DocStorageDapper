using Dapper;
using DocStorage.Domain;
using DocStorage.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connection;

        #region Query List

        private const string SELECT_ALL = @"
            SELECT 
                * 
            FROM 
                public.user";


        private const string SELECT_BY_ID = @"
            SELECT 
                * 
            FROM 
                public.user 
            WHERE 
                public.user.user_id = @UserId";


        private const string DELETE = @"
            DELETE FROM 
                public.user 
            WHERE 
                public.user.user_id = @UserId";

        private const string INSERT = @"
            INSERT INTO 
                public.user (user_name, password_hash, role) 
            VALUES (@UserName, @PasswordHash, @Role)";

        private const string SELECT_BY_USERNAME = @"
            SELECT 
                * 
            FROM 
                public.user 
            WHERE 
                public.user.user_name = @UserName";

        #endregion


        public UserRepository(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public List<User> GetAll()
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<User>(SELECT_ALL).ToList();
            }
        }

        public User Get(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.QuerySingleOrDefault<User>(SELECT_BY_ID, new { UserId = id });
            }
        }

        public bool Add(Model.User entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(INSERT, new { UserName = entity.UserName, PasswordHash = entity.Password, Role = entity.Role }) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(DELETE, new { UserId = id }) > 0;
            }
        }

        public User GetByUsername(string username)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.QuerySingleOrDefault<User>(SELECT_BY_USERNAME, new { UserName = username });
            }
        }
    }
}
