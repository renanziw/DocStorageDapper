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
    public class DocumentAccessRepository : IDocumentAccessRepository
    {
        private readonly IConnectionFactory _connection;

        #region Query List

        private const string SELECT_ALL = @"
            SELECT 
	            public.documentaccess.document_access_id,
	            public.document.*,
	            public.group.*,
	            public.user.*
            FROM 
	            public.documentaccess
	            left join public.document on (public.document.document_id = public.documentaccess.document_id)
	            left join public.group on (public.group.group_id = public.documentaccess.group_id)
	            left join public.user on (public.user.user_id = public.documentaccess.user_id)";


        private const string SELECT_BY_ID = @"
            SELECT 
	            public.documentaccess.document_access_id,
	            public.document.*,
	            public.group.*,
	            public.user.*
            FROM 
	            public.documentaccess
	            left join public.document on (public.document.document_id = public.documentaccess.document_id)
	            left join public.group on (public.group.group_id = public.documentaccess.group_id)
	            left join public.user on (public.user.user_id = public.documentaccess.user_id)
            WHERE 
                public.documentaccess.document_access_id = @Id";


        private const string DELETE = @"
            DELETE FROM 
                public.documentaccess 
            WHERE 
                public.documentaccess.document_access_id = @Id";

        private const string INSERT = @"
            INSERT INTO 
                public.documentaccess (document_id, group_id, user_id) 
            VALUES (@DocumentId, @GroupId, @UserId)";

        private const string SELECT_BY_DOCUMENT_ID = @"
            SELECT 
	            public.documentaccess.document_access_id,
	            public.document.*,
	            public.group.*,
	            public.user.*
            FROM 
	            public.documentaccess
	            left join public.document on (public.document.document_id = public.documentaccess.document_id)
	            left join public.group on (public.group.group_id = public.documentaccess.group_id)
	            left join public.user on (public.user.user_id = public.documentaccess.user_id) 
            WHERE 
                public.documentaccess.document_id = @Id";

        #endregion

        public DocumentAccessRepository(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public List<DocumentAccess> GetAll()
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<DocumentAccess, Document, Group, User, DocumentAccess>(SELECT_ALL,
                    map: (documentAccess, document, group, user) => {
                        documentAccess.Document = document;
                        documentAccess.Group = group;
                        documentAccess.User = user;

                        return documentAccess;
                    },
                    splitOn: "document_id,group_id,user_id").ToList();
            }
        }

        public DocumentAccess Get(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<DocumentAccess, Document, Group, User, DocumentAccess>(SELECT_BY_ID,
                    map: (documentAccess, document, group, user) => {
                        documentAccess.Document = document;
                        documentAccess.Group = group;
                        documentAccess.User = user;

                        return documentAccess;
                    },
                    param: new { Id = id },
                    splitOn: "document_id,group_id,user_id").FirstOrDefault();
            }
        }

        public bool Add(Model.DocumentAccessCreateModel entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(INSERT, new { DocumentId = entity.DocumentId, GroupId = entity.GroupId, UserId = entity.UserId }) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                try 
                { 
                    return connectionDb.Execute(DELETE, new { Id = id }) > 0;
                }
                catch (NpgsqlException ex)
                {
                    //log
                    return false;
                }
            }
        }

        public List<DocumentAccess> GetAllByDocumentId(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<DocumentAccess, Document, Group, User, DocumentAccess>(SELECT_BY_DOCUMENT_ID,
                    map: (documentAccess, document, group, user) => {
                        documentAccess.Document = document;
                        documentAccess.Group = group;
                        documentAccess.User = user;

                        return documentAccess;
                    },
                    param: new { Id = id },
                    splitOn: "document_id,group_id,user_id").ToList();
            }
        }
    }
}
