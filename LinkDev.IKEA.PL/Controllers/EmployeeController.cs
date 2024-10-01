using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.BLL.Services.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    // Inheritance : DepartmentController is a Controller
    // Composition : DepartmentController has a IDepartmentService
    [Authorize]
    public class EmployeeController : Controller
    {
        #region First Way 
        //[FromServices]
        //public IDepartmentService DepartmentService { get; } = null!;
        #endregion

        #region Services
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _environment;
        public EmployeeController(
            IEmployeeService employeeService,
             ILogger<EmployeeController> logger,
            IWebHostEnvironment environment)
        {
            _employeeService = employeeService;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region Index

        [HttpGet] // GET : /Employee/Index
        public async Task<IActionResult> Index(string search)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(search);

            return View(employees);
        }

        #endregion

        #region Details
        [HttpGet] // GET : /employee/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (employee is null)
                return NotFound();

            return View(employee);
        }

        #endregion

        #region Create
        [HttpGet] // GET : /Employee/Create
        public IActionResult Create()
        {
 

            return View();
        }

        [HttpPost] // POST
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedEmployeeDto employee)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(employee);

            var message = string.Empty;
            try
            {
                var result = await _employeeService.CreateEmployeeAsync(employee);
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    message = "Employee Is Not Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);

                // 2. Set Message
                message = _environment.IsDevelopment() ? ex.Message : "An Error During Creating The Employee :(";

            }

            ModelState.AddModelError(string.Empty, message);
            return View(employee);

        }


        #endregion   [HttpGet] // Get: Employee/Edit/id

        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest(); // 400

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (employee is null)
                return NotFound(); //404


            return View(new UpdatedEmployeeDto()
            {
                Name = employee.Name,
                Address = employee.Address,
                Email = employee.Email,
                Age = employee.Age,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                EmployeeType = employee.EmployeeType,
                Gender= employee.Gender,
                HiringDate = employee.HiringDate,
            });
        }

        [HttpPost] //Post
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDto employee)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(employee);

            var message = string.Empty;

            try
            {
              

                var Updated = await _employeeService.UpdateEmployeeAsync(employee) > 0;

                if (Updated)
                    return RedirectToAction(nameof(Index));


                message = "An Error During Updating The Employee :(";


            }
            catch (Exception ex)
            {

                // 1. Log Exception
                _logger.LogError(ex, ex.Message);

                // 2. Set Message

                message = _environment.IsDevelopment() ? ex.Message : "An Error During Updating The Employee :(";


            }

            ModelState.AddModelError(string.Empty, message);
            return View(employee);


        }

        #endregion

        #region Delete
   
        [HttpPost] //Post
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Delete action called with ID: {id}");

            var message = string.Empty;

            try
            {
                var deleted = await _employeeService.DeleteEmployeeAsync(id);
                if (deleted)
                    return RedirectToAction(nameof(Index));


                message = "An Error During Deleting The Employee :(";
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);

                // 2. Set Message

                message = _environment.IsDevelopment() ? ex.Message : "An Error During Deleting The Employee :(";
            }
            //ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Index));

        }

        #endregion

    }
}
