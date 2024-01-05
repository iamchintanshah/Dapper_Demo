using LearnDapper.Domain.Model;

namespace LearnDapper.Domain.Interfaces;

public interface IEmployeeRepository
{
    //Task<List<Employee>> GetAll();
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee> GetEmployeeById(int id);
    Task<int> Create(Employee employee);
    Task<string> Update(Employee employee, int id);
    Task<string> Remove(int id);
}
