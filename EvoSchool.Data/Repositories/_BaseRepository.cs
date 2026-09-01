using System;
using System.Data;
using System.Data.SqlClient;

namespace EvoSchool.Data.Repositories
{
    public abstract class _BaseRepository
    {
        private readonly string _connectionString;

        protected _BaseRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
