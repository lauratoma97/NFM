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
        private readonly IPhotoUploadService _photoUploadService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmployeeController(IEmployeeService employeeService, IValidator<EmployeeDto> employeeValidator, IPhotoUploadService photoUploadService, IWebHostEnvironment hostingEnvironment)
        {
            _employeeService = employeeService;
            _employeeValidator = employeeValidator;
            _photoUploadService = photoUploadService;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null || id != employeeDto.Id)
            {
                return BadRequest("Employee data is invalid or ID mismatch.");
            }

            var validationResult = await _employeeValidator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var existingEmployee = await _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            await _employeeService.UpdateEmployee(employeeDto);
            return NoContent();
        }

        [HttpPost("{id}/photo")]
        public async Task<IActionResult> UploadPhoto(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            var photoPath = await _photoUploadService.UploadPhotoAsync(file);
            employee.PhotoPath = photoPath;
            await _employeeService.UpdateEmployee(employee);

            return Ok(new { photoUrl = employee.PhotoPath });
        }

        [HttpGet("{id}/photo")]
        public async Task<IActionResult> DownloadPhoto(long id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null || string.IsNullOrEmpty(employee.PhotoPath))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, employee.PhotoPath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);

            return File(imageBytes, "image/jpeg", fileDownloadName: fileName);
        }
    }
}