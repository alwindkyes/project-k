using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace project_k.Controllers;

[ApiController]

public class EmployeeController : ControllerBase
{
    private readonly LearningDbContext _learningDbContext;

    public EmployeeController(LearningDbContext learningDbContext)
    {
        _learningDbContext = learningDbContext;
    }

    [HttpPost("employee")]
    public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
    {
        EmployeeDataModel employeeDataModel = new EmployeeDataModel
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Country = employee.Country
        };
        await _learningDbContext.Employee.AddAsync(employeeDataModel);
        await _learningDbContext.SaveChangesAsync();
        return Ok(employeeDataModel);
    }

    [HttpGet("employeeList")]

    public async Task<IActionResult> GetEmployeeList()
    {
        List<EmployeeDataModel> employeeDataList = await _learningDbContext.Employee.ToListAsync();
        List<Employee> employeeList = new List<Employee>();
        foreach (EmployeeDataModel item in employeeDataList)
        {
            Employee employee = new Employee
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Country = item.Country
            };
            employeeList.Add(employee);
        }
        return Ok(employeeList);
    }

    [HttpGet("employee/{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        EmployeeDataModel employeeDataModel = await _learningDbContext.Employee.FirstOrDefaultAsync(e => e.Id == id);
        if (employeeDataModel == null)
        {
            return NotFound(new StatusResponse(404, "Not Found"));
        }
        Employee employee = new Employee
        {
            FirstName = employeeDataModel.FirstName,
            LastName = employeeDataModel.LastName,
            Email = employeeDataModel.Email,
            Country = employeeDataModel.Country
        };
        return Ok(employee);
    }

    [HttpPut("employee/{id}")]

    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
    {
        EmployeeDataModel employeeToUpdate = await _learningDbContext.Employee.FirstOrDefaultAsync(e => e.Id == id);
        if (employeeToUpdate == null)
        {
            return NotFound(new StatusResponse(404, "Not Found"));
        }
        employeeToUpdate.FirstName = employee.FirstName;
        employeeToUpdate.LastName = employee.LastName;
        employeeToUpdate.Email = employee.Email;
        employeeToUpdate.Country = employee.Country;
        await _learningDbContext.SaveChangesAsync();
        return Ok(new StatusResponse(200, "Successfully Updated"));
    }

    [HttpDelete("employee/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        EmployeeDataModel employeeToDelete = await _learningDbContext.Employee.FirstOrDefaultAsync(e => e.Id == id);
        if (employeeToDelete == null)
        {
            return NotFound(new StatusResponse(404, "Not Found"));
        }
        _learningDbContext.Employee.Remove(employeeToDelete);
        await _learningDbContext.SaveChangesAsync();
        return Ok(new StatusResponse(200, "Successfully Deleted"));
    }
}