using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;

        public DepartmentController(IDepartmentService departmentService
            ,ILogger<DepartmentController>logger
            ,IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
            _departmentService = departmentService;
          
        }

        [HttpGet]
        public IActionResult Index()
        {

            var departments = _departmentService.GetAllDepartments();

            return View(departments);
        }




        [HttpGet] //Get
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost] //Post
        public IActionResult Create(CreatedDepartmentDto department)
        {
            if (ModelState.IsValid)
                return View(department);

            var message = string.Empty;


            try
            {
                var result = _departmentService.CreateDepartment(department);

                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                { 
                    message = "Department Is Not Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(department);
                }
            }
            catch (Exception ex)
            {
               // 1. Log Exception

                _logger.LogError(ex, ex.Message);


                // 2. Set Message

                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(department);
                }
                else
                {
                    message = "Department Is Not Created";

                    return View("Error", message);
                }
                   

               

            }


        }

    }
}
