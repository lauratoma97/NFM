using NFM.Business.ModelDTOs;

namespace NFM.Business.Services.Contracts
{
    public interface IEmployeeService
    {
        public Task<List<EmployeeDto>> GetEmployees();

        public Task<EmployeeDto?> GetEmployeeById(long id);

        public Task<long> CreateEmployee(EmployeeDto employeeDto);

        public Task DeleteEmployee(long id);

        public Task UpdateEmployee(EmployeeDto employeeDto);
    }
}
