using System.Data;
using Npgsql;

namespace WebServerHB.HelpEntities;

public static class NpgSqlEx
{
    public static async Task OpenConnectionIfClosed(this NpgsqlConnection connection)
    {
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
    }
}