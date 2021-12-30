using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;

namespace DataAccess.DbAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config )
    {
        _config = config;
    }

    public async Task SaveData<T>(
        string storedProcedure,
        T parameters,
        string connectionId = "Default"
    )

    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

        await connection.ExecuteAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

    }
}
