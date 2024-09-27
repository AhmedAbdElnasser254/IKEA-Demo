using AutoMapper;
using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.DAL.Entities.Department;
using LinkDev.IKEA.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    // Inheritance : DepartmentController is a Controller
    // Composition : DepartmentController has a IDepartmentService
    public class DepartmentController : Controller
    {
        #region First Way 
        //[FromServices]
        //public IDepartmentService DepartmentService { get; } = null!;
        #endregion

        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;
        public DepartmentController(IDepartmentService departmentService,
            IMapper mapper,
            ILogger<DepartmentController> logger,
            IWebHostEnvironment environment)
        {
            _departmentService = departmentService;
            _mapper = mapper;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region Index
        [HttpGet] // GRT : /Department/Index
        public IActionResult Index()
        {

            // Views Dictionary : Pass Data From Controller(Action) To View (From View ---> [Partial View , Layout])



            // 1 . ViewData is a Dictionary Type Property
            ViewData["Message"] = "Hello Ahmed";


            // 2. ViewBag is a Dynamic Type Property
            ViewBag.Message = "Hello Ahmed";
            ViewBag.Message = new { Id = 10, Name = "Ahmed" };



            var departments = _departmentService.GetAllDepartments();

            return View(departments);
        }

        #endregion

        #region Details
        [HttpGet] // GET : /Department/Details
        public IActionResult Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = _departmentService.GetDepartmentById(id.Value);

            if (department is null)
                return NotFound();

            return View(department);
        }

        #endregion

        #region Create
        [HttpGet] // GET : /Department/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] // POST
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(departmentVM);

            var message = string.Empty;
            try
            {
                ///var CreatedDepartment = new CreatedDepartmentDto()
                ///{
                ///
                ///    Code = departmentVM.Code,
                ///    Name = departmentVM.Name,
                ///    Description = departmentVM.Description,
                ///    CreationDate = departmentVM.CreationDate,
                ///
                ///};
                ///

                var CreatedDepartment = _mapper.Map<CreatedDepartmentDto>(departmentVM);


                var Created = _departmentService.CreateDepartment(CreatedDepartment) > 0;

                // TempData : Is a Property of type Dictionary object

                if (!Created)
                    message = "Department Is Created";

                ModelState.AddModelError(string.Empty, message);
                return View(departmentVM);


              
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);

                // 2. Set Message
                message = _environment.IsDevelopment() ? ex.Message : "An Error During Creating The Department :(";

                TempData["Message"] = message;
                return RedirectToAction(nameof(Index));


            }

        }


        #endregion   [HttpGet] // Get: Department/Edit/id

        #region Update
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest(); // 400

            var department = _departmentService.GetDepartmentById(id.Value);

            if (department is null)
                return NotFound(); //404


            var departmentVM = _mapper.Map<DepartmentDetailsDto, DepartmentEditViewModel>(department);


            return View(departmentVM);
        }

        [HttpPost] //Post
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(departmentVM);

            var message = string.Empty;


            try
            {

                ///var departmentToUpdate = new UpdatedDepartmentDto()
                ///{
                ///    Id = id,
                ///    Code = departmentVM.Code,
                ///    Name = departmentVM.Name,
                ///    Description = departmentVM.Description,
                ///    CreationDate = departmentVM.CreationDate,};

                var departmentToUpdate = _mapper.Map<UpdatedDepartmentDto>(departmentVM);


                var Updated = _departmentService.UpdateDepartment(departmentToUpdate) > 0;

                if (Updated)
                    return RedirectToAction(nameof(Index));


                message = "An Error During Updating The Department :(";


            }
            catch (Exception ex)
            {

                // 1. Log Exception
                _logger.LogError(ex, ex.Message);

                // 2. Set Message

                message = _environment.IsDevelopment() ? ex.Message : "An Error During Updating The Department :(";


            }

            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);


        }

        #endregion

        #region Delete
        [HttpGet] // Get /Department/Delete/id?
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = _departmentService.GetDepartmentById(id.Value);

            if (department is null)
                return NotFound();

            return View(department);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;

            try
            {
                var deleted = _departmentService.DeleteDepartment(id);
                if (deleted)
                    return RedirectToAction(nameof(Index));


                message = "An Error During Deleting The Department :(";
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);

                // 2. Set Message

                message = _environment.IsDevelopment() ? ex.Message : "An Error During Deleting The Department :(";
            }
            //ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Index));


        }

        #endregion
    }
}
