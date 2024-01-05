using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace LearnDapper.Data;

public class DapperDBContext
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionstring;

    public DapperDBContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionstring = _configuration.GetConnectionString("connection");
    }
    public IDbConnection CreateConnection() => new SqlConnection(_connectionstring);

    //public IDbConnection CreateConnection() => new SqlConnection("Server=CHINTAN-PC;Database=Learn_Dapper;Trusted_Connection=True;TrustServerCertificate=True");
}
