using AutoMapper;
using NFM.Business.ModelDTOs;
using NFM.Business.Services.Contracts;
using NFM.Domain.Models;
using NFM.Domain.Repositories;

namespace NFM.Business.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            var employeeEntities = await _employeeRepository.Get();
            return employeeEntities.Select(e => _mapper.Map<EmployeeDto>(e)).ToList();
        }

        public async Task<EmployeeDto?> GetEmployeeById(long id)
        {
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
            {
                return null;
            }

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<long> CreateEmployee(EmployeeDto employeeDto)
        {
            if (employeeDto != null)
            {
                var employeeEntity = _mapper.Map<Employee>(employeeDto);
                await _employeeRepository.Add(employeeEntity);
                
                return employeeEntity.Id;
            }

            return 0;
        }

        public async Task DeleteEmployee(long id)
        {
            await _employeeRepository.Delete(id);
        }
    }
}
