using Dapper;
using DocStorage.Domain;
using DocStorage.Repository.Contracts;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Repository.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IConnectionFactory _connection;

        #region Query List

        private const string SELECT_ALL = @"
            SELECT 
                * 
            FROM 
                public.group";


        private const string SELECT_BY_ID = @"
            SELECT 
                * 
            FROM 
                public.group 
            WHERE 
                public.group.group_id = @Id";


        private const string DELETE = @"
            DELETE FROM 
                public.group 
            WHERE 
                public.group.group_id = @Id";

        private const string INSERT = @"
            INSERT INTO 
                public.group (name) 
            VALUES (@Name)";

        #endregion

        public GroupRepository(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public List<Group> GetAll()
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<Group>(SELECT_ALL).ToList();
            }
        }

        public Group Get(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.QuerySingleOrDefault<Group>(SELECT_BY_ID, new { Id = id });
            }
        }

        public bool Add(Model.Group entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(INSERT, new { Name = entity.Name }) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                try
                {
                    return connectionDb.Execute(DELETE, new { Id = id }) > 0;
                } catch(NpgsqlException ex)
                {
                    //log
                    return false;
                }
            }
        }
    }
}
