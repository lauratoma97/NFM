using Microsoft.EntityFrameworkCore;
using NFM.Domain.Context;
using NFM.Domain.Models;

namespace NFM.Domain.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyDbContext _dbContext;

        public EmployeeRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> Get()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<Employee?> GetById(long id)
        {
            return (await _dbContext.Employees.FindAsync(id))!;
        }

        public async Task<long> Add(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            return employee.Id;
        }

        public async Task Delete(long id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> CnpAlreadyExist(string cnp)
        {
            return await _dbContext.Employees.AnyAsync(e => e.CNP == cnp);
        }

        public async Task Update(Employee employee)
        {

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
