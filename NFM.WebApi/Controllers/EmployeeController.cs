using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NFM.Business.ModelDTOs;
using NFM.Business.Services.Contracts;

namespace NFM.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<EmployeeDto> _employeeValidator;

        public EmployeeController(IEmployeeService employeeService, IValidator<EmployeeDto> employeeValidator)
        {
            _employeeService = employeeService;
            _employeeValidator = employeeValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();
           
            return Ok(employees);
        }

        [HttpGet("{id}", Name = nameof(GetEmployeeById))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(long id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeDto employee)
        {
            var validationResult = await _employeeValidator.ValidateAsync(employee);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdEmployeeId = await _employeeService.CreateEmployee(employee);

            return CreatedAtRoute(nameof(GetEmployeeById), new { id = createdEmployeeId }, createdEmployeeId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteEmployee(id);
            
            return NoContent();
        }
    }
}