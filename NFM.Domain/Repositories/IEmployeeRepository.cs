using NFM.Domain.Models;

namespace NFM.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> Get();

        Task<Employee?> GetById(long id);

        Task<long> Add(Employee employee);

        Task Delete(long id);

        Task<bool> CnpAlreadyExist(string cnp);
    }
}
