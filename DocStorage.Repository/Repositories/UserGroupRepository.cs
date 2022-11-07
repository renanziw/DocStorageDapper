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
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly IConnectionFactory _connection;

        #region Query List

        private const string SELECT_ALL = @"
            SELECT 
	            public.usergroup.user_group_id  AS user_group_id,
	            public.group.group_id           AS group_id,
	            public.group.name               AS name,
	            public.user.user_id             AS user_id,
	            public.user.user_name           AS user_name,
	            public.user.role                AS role
            FROM 
	            public.usergroup 
	            INNER JOIN public.group ON (public.usergroup.group_id = public.group.group_id)
	            INNER JOIN public.user  ON (public.usergroup.user_id = public.user.user_id)";


        private const string SELECT_BY_ID = @"
            SELECT 
	            public.usergroup.user_group_id  AS user_group_id,
	            public.group.group_id           AS group_id,
	            public.group.name               AS name,
	            public.user.user_id             AS user_id,
	            public.user.user_name           AS user_name,
	            public.user.role                AS role
            FROM 
	            public.usergroup 
	            INNER JOIN public.group ON (public.usergroup.group_id = public.group.group_id)
	            INNER JOIN public.user  ON (public.usergroup.user_id = public.user.user_id)
            WHERE 
                public.usergroup.user_group_id = @Id";


        private const string DELETE = @"
            DELETE FROM 
                public.usergroup 
            WHERE 
                public.usergroup.user_group_id = @Id";

        private const string INSERT = @"
            INSERT INTO 
                public.usergroup (user_id, group_id) 
            VALUES 
                (@UserId, @GroupId)";

        private const string SELECT_BY_USER_ID = @"
           SELECT 
	            public.usergroup.user_group_id  AS user_group_id,
	            public.group.group_id           AS group_id,
	            public.group.name               AS name,
	            public.user.user_id             AS user_id,
	            public.user.user_name           AS user_name,
	            public.user.role                AS role
            FROM 
	            public.usergroup 
	            INNER JOIN public.group ON (public.usergroup.group_id = public.group.group_id)
	            INNER JOIN public.user  ON (public.usergroup.user_id = public.user.user_id)
            WHERE 
	            public.usergroup.user_id = @Id";

        #endregion

        public UserGroupRepository(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public List<UserGroup> GetAll()
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<UserGroup, Group, User, UserGroup>(SELECT_ALL,
                    map: (userGroup, group, user) => {
                        userGroup.Group = group;
                        userGroup.User = user;

                        return userGroup;
                    },
                    splitOn: "group_id,user_id").ToList();
            }
        }

        public UserGroup Get(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<UserGroup, Group, User, UserGroup>(SELECT_BY_ID,
                    map: (userGroup, group, user) => {
                        userGroup.Group = group;
                        userGroup.User = user;

                        return userGroup;
                    },
                    param: new { Id = id },
                    splitOn: "group_id,user_id").FirstOrDefault();
            }
        }

        public bool Add(Model.UserGroupCreateModel entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(INSERT, new { UserId = entity.UserId, GroupId = entity.GroupId }) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(DELETE, new { Id = id }) > 0;
            }
        }

        public List<UserGroup> GetAllByUserId(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<UserGroup, Group, User, UserGroup>(SELECT_BY_USER_ID,
                    map:(userGroup, group, user) => {
                        userGroup.Group = group;
                        userGroup.User = user;

                        return userGroup;
                    },
                    param: new { Id = id },
                    splitOn: "group_id,user_id").ToList();
            }
        }
    }
}
