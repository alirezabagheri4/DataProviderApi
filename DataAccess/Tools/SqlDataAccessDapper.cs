using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Tools
{
    public class SqlDataAccessDapper:ISqlDataAccessDapper
    {
        private readonly IConfiguration _config;
        public SqlDataAccessDapper(IConfiguration config)
        {
            _config = config;
        }

        private const string  ConnectionString =
            "Data Source=DESKTOP-3DPTK00;Initial Catalog=DataProvider;Integrated Security=True;";
        public async Task<IEnumerable<T>> LoadData<T, TU>(
            string storedProcedure,
            TU parameters)
        {
            using IDbConnection connection = new SqlConnection(ConnectionString);

            return await connection.QueryAsync<T>(storedProcedure, parameters,
                commandType: CommandType.Text);
        }

        public async Task SaveData<T>(
            string storedProcedure,
            T parameters)
        {
            using IDbConnection connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync(storedProcedure, parameters,
                commandType: CommandType.Text);
        }
    }
}
