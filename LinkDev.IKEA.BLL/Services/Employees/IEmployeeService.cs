using LinkDev.IKEA.BLL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    internal interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployees();


        EmployeeDetailsDto? GetEmployeeById(int id);

        int CreateEmployee(CreatedEmployeeDto employeeDto);

        int UpdateEmployee(UpdatedEmployeeDto employeeDto);
        bool DeleteEmployee(int id);
    }
}
