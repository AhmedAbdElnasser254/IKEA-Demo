using LinkDev.IKEA.BLL.Common.Attachments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.BLL.Services.Employees;
using LinkDev.IKEA.DAL.Models.Identity;
using LinkDev.IKEA.DAL.Persistance.Data;
using LinkDev.IKEA.DAL.Persistance.Repositories.Departments;
using LinkDev.IKEA.DAL.Persistance.Repositories.Employees;
using LinkDev.IKEA.DAL.Persistance.UnitOfWork;
using LinkDev.IKEA.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LinkDev.IKEA.PL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configure Services


			// Add services to the container.
			builder.Services.AddControllersWithViews();


			builder.Services.AddDbContext<ApplicationDbContext>((optionBuilder) =>
			{

				optionBuilder
				.UseLazyLoadingProxies()
				.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

			});





			//builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



			builder.Services.AddScoped<IDepartmentService, DepartmentService>();
			builder.Services.AddScoped<IEmployeeService, EmployeeService>();
			builder.Services.AddTransient<IAttachmentService, AttachmentService>();




			//builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));


			builder.Services.AddIdentity<ApplicationUser, IdentityRole>((option) =>
			{
				option.Password.RequiredLength = 5;
				option.Password.RequireDigit = true;
				option.Password.RequireNonAlphanumeric = true; // *#%&
				option.Password.RequireUppercase = true;
				option.Password.RequireLowercase = true;
				option.Password.RequiredUniqueChars = 1;

				option.User.RequireUniqueEmail = true;
				//option.User.AllowedUserNameCharacters = "asmdma;asdms";


				option.Lockout.AllowedForNewUsers = true;
				option.Lockout.MaxFailedAccessAttempts = 5;
				option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);





			})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.ConfigureApplicationCookie((option) =>
			{
				option.LoginPath = "/Account/SignIn";
				option.AccessDeniedPath = "/Home/Error";
				option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
				option.LogoutPath = "/Account/SignIn";
			});

			builder.Services.AddAuthentication(option =>
			{

				option.DefaultAuthenticateScheme = "Identity.Application";
				option.DefaultChallengeScheme = "Identity.Application";

			})
				.AddCookie("Hamda", ".AspNetCore.Hamda", option => 
				{
					option.LoginPath = "/Account/Login";
					option.AccessDeniedPath = "/Home/Error";
					option.ExpireTimeSpan = TimeSpan.FromMinutes(5);

					//option.LogoutPath = "/Account/SignIn";
					//option.ForwardSignOut = "/Account/SignIn";

                });

			#endregion

			var app = builder.Build();

			#region Configure Kestrel Middlewares

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			#endregion

			app.Run();
		}
	}
}
