using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.BLL.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    // Inheritance : DepartmentController is a Controller
    // Composition : DepartmentController has a IDepartmentService
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
        public EmployeeController(IEmployeeService employeeService,
            ILogger<EmployeeController> logger,
            IWebHostEnvironment environment)
        {
            _employeeService = employeeService;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region Index
        [HttpGet] // GRT : /employee/Index
        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
            return View(employees);
        }

        #endregion

        #region Details
        [HttpGet] // GET : /employee/Details
        public IActionResult Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = _employeeService.GetEmployeeById(id.Value);

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
        public IActionResult Create(CreatedEmployeeDto employee)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(employee);

            var message = string.Empty;
            try
            {
                var result = _employeeService.CreateEmployee(employee);
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

        #region Edit
        //public IActionResult Edit(int? id)
        //{
        //    if (id is null)
        //        return BadRequest(); // 400

        //    var employee = _employeeService.GetEmployeeById(id.Value);

        //    if (employee is null)
        //        return NotFound(); //404

        //    return View(new EmployeeEditViewModel()
        //    {

        //        Code = employee.Code,
        //        Name = employee.Name,
        //        Description = employee.Description,
        //        CreationDate = employee.CreationDate,
        //    });
        //}

        //[HttpPost] //Post
        //public IActionResult Edit([FromRoute] int id, EmployeeEditViewModel employeeVM)
        //{
        //    if (!ModelState.IsValid) // Server-Side Validation
        //        return View(employeeVM);

        //    var message = string.Empty;


        //    try
        //    {

        //        var employeeToUpdate = new UpdatedEmployeeDto()
        //        {
        //            Id = id,
        //            Code = employeeVM.Code,
        //            Name = employeeVM.Name,
        //            Description = employeeVM.Description,
        //            CreationDate = employeeVM.CreationDate,

        //        };

        //        var Updated = _employeeService.UpdateEmployee(employeeToUpdate) > 0;

        //        if (Updated)
        //            return RedirectToAction(nameof(Index));


        //        message = "An Error During Updating The Employee :(";


        //    }
        //    catch (Exception ex)
        //    {

        //        // 1. Log Exception
        //        _logger.LogError(ex, ex.Message);

        //        // 2. Set Message

        //        message = _environment.IsDevelopment() ? ex.Message : "An Error During Updating The Employee :(";


        //    }

        //    ModelState.AddModelError(string.Empty, message);
        //    return View(employeeVM);


        //}

        #endregion

        #region Delete
        [HttpGet] // Get /Employee/Delete/id?
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = _employeeService.GetEmployeeById(id.Value);

            if (employee is null)
                return NotFound();

            return View(employee);

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;

            try
            {
                var deleted = _employeeService.DeleteEmployee(id);
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
