using LinkDev.IKEA.DAL.Models.Identity;
using LinkDev.IKEA.PL.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
	public class AccountController : Controller
	{
		#region Services
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		} 
		#endregion



		#region Sign Up


		[HttpGet] // Get: /Account/SignUp
		public IActionResult SignUp()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = await _userManager.FindByNameAsync(model.UserName);

			if (user is { })
			{
				ModelState.AddModelError(nameof(SignUpViewModel.UserName), "This Username Is Already Used");
				return View(model);
			}


			if (user is null)
			{
				user = new ApplicationUser()
				{
					FName = model.FirstName,
					LName = model.LastName,
					UserName = model.UserName,
					Email = model.Email,
					IsAgree = model.IsAgree,

				};


				var result = await _userManager.CreateAsync(user, model.Password);


				if (result.Succeeded)
					return RedirectToAction(nameof(SignIn));

                foreach (var error in result.Errors)
                {
					ModelState.AddModelError(string.Empty, error.Description);
                }

            }
			

		  return View(model);

		}



		#endregion



		#region SignIn


		public IActionResult SignIn()
		{
			return View();			
		}


		#endregion

	}
}
