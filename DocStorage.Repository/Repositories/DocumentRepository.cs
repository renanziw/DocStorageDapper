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
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IConnectionFactory _connection;

        #region Query List

        private const string SELECT_ALL = @"
            SELECT 
                * 
            FROM 
                public.document";


        private const string SELECT_BY_ID = @"
            SELECT 
                * 
            FROM 
                public.document 
            WHERE 
                public.document.document_id = @Id";


        private const string DELETE = @"
            DELETE FROM 
                public.document 
            WHERE 
                public.document.document_id = @Id";

        private const string INSERT = @"
            INSERT INTO 
                public.document (name, description, category, posted_date, extension) 
            VALUES (@Name, @Description, @Category, @PostedDate, @Extension)";

        #endregion

        public DocumentRepository(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public List<Document> GetAll()
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Query<Document>(SELECT_ALL).ToList();
            }
        }

        public Document Get(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.QuerySingleOrDefault<Document>(SELECT_BY_ID, new { Id = id });
            }
        }

        public bool Add(Model.Document entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(INSERT, 
                    new 
                    { 
                        Name = entity.Name, 
                        Description = entity.Description, 
                        Category = entity.Category, 
                        PostedDate = entity.PostedDate, 
                        Extension = entity.Extension 
                    }) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connectionDb = _connection.Connection())
            {
                return connectionDb.Execute(DELETE, new { Id = id }) > 0;
            }
        }
    }
}
