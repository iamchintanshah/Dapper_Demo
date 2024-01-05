using Dapper;
using LearnDapper.Domain.Interfaces;
using LearnDapper.Domain.Model;
using System.Data;

namespace LearnDapper.Data.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DapperDBContext _context;

    public EmployeeRepository(DapperDBContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Employee employee)
    {
        string SP_INSERT = "sp_InsertUpdateEmployee";
        var parameters = new { employee.Id, employee.Name, employee.Email, employee.Phone, employee.Designation };
        using (var connection = _context.CreateConnection())
        {
            var insert = await connection.QueryAsync<int>(SP_INSERT, parameters, commandType: CommandType.StoredProcedure);
            return insert.Single();
        }
    }

    public async Task<string> Update(Employee employee, int id)
    {
        #region Update using Query...
        {
            //string response = string.Empty;
            //string query = "update Employee set Name=@name,Email=@email,Phone=@phone,Designation=@designation where id=@id";
            //var parameters = new DynamicParameters();
            //parameters.Add("id", id, DbType.Int32);
            //parameters.Add("name", employee.Name, DbType.String);
            //parameters.Add("email", employee.Email, DbType.String);
            //parameters.Add("phone", employee.Phone, DbType.String);
            //parameters.Add("designation", employee.Designation, DbType.String);
            //using (var connection = _context.CreateConnection())
            //{
            //    await connection.ExecuteAsync(query, parameters);
            //    response = "pass";
            //}
            //return response;
        };
        #endregion

        try
        {
            string SP_INSERT = "sp_InsertUpdateEmployee";
            var parameters = new { id, employee.Name, employee.Email, employee.Phone, employee.Designation };
            using (var connection = _context.CreateConnection())
            {
                var update = await connection.QueryAsync(SP_INSERT, parameters, commandType: CommandType.StoredProcedure);
            }
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

    //public async Task<List<Employee>> GetAll()
    public async Task<IEnumerable<Employee>> GetAll()
    {
        string query = "Select * From Employee";
        using (var connection = _context.CreateConnection())
        {
            var emplist = await connection.QueryAsync<Employee>(query);
            return emplist.ToList();
        }
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        string query = "Select * From Employee where id=@id";
        using (var connection = _context.CreateConnection())
        {
            var emplist = await connection.QueryFirstOrDefaultAsync<Employee>(query, new { id });
            return emplist ?? new Employee();
        }
    }

    public async Task<string> Remove(int id)
    {
        string response = string.Empty;
        string query = "Delete From Employee where id=@id";
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { id });
            response = "Success";
        }
        return response;
    }
}
