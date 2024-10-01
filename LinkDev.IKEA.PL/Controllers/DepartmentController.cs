using AutoMapper;
using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
	// Inheritance : DepartmentController is a Controller
	// Composition : DepartmentController has a IDepartmentService
	[Authorize]
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
        public async Task<IActionResult> Index()
        {

            #region ViewData Vs ViewBag
            //// Views Dictionary : Pass Data From Controller(Action) To View (From View ---> [Partial View , Layout])



            //// 1 . ViewData is a Dictionary Type Property
            //ViewData["Message"] = "Hello Ahmed";


            //// 2. ViewBag is a Dynamic Type Property
            //ViewBag.Message = "Hello Ahmed";
            //ViewBag.Message = new { Id = 10, Name = "Ahmed" }; 
            #endregion


            var departments = await _departmentService.GetAllDepartmentsAsync();
             
            return View(departments);

        }

        #endregion

        #region Details
        [HttpGet] // GET : /Department/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

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
        public async Task<IActionResult> Create(DepartmentEditViewModel departmentVM)
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


                var Created = await _departmentService.CreateDepartmentAsync(CreatedDepartment) > 0;

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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest(); // 400

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

            if (department is null)
                return NotFound(); //404


            var departmentVM = _mapper.Map<DepartmentDetailsDto, DepartmentEditViewModel>(department);


            return View(departmentVM);
        }

        [HttpPost] //Post
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentEditViewModel departmentVM)
        { 
            if(id is null)
                return BadRequest();

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


                var Updated = await _departmentService.UpdateDepartmentAsync(departmentToUpdate);

                if (Updated > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else 
                {
                    message = "An Error During Updating The Department :(";
                }


               



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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

            if (department is null)
                return NotFound();

            return View(department);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Delete(int id)
        {
            var message = string.Empty;

            try
            {
                var isdeleted = await _departmentService.DeleteDepartmentAsync(id);
                if (isdeleted == true)
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
