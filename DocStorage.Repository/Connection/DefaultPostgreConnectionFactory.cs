using Dapper;
using DocStorage.Repository.Contracts;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Repository.Connection
{
    public class DefaultPostgreConnectionFactory : IConnectionFactory
    {
        public IDbConnection Connection()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return new NpgsqlConnection("Server=localhost;Database=docstorage;Port=5432;User Id=user;Password=user;");
        }

        public void CreateInitialDatabase()
        {
            using(var db = Connection())
            {
                db.Execute(Resource.createstructure);
            }
        }
    }
}
