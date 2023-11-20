using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcProject.DTO;
using System.Security.Claims;
using MvcProject.Data;
using MvcProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace MvcProject.Controllers
{
	public class AccountController : Controller
	{

		private readonly ApplicationDbContext dbContext;

		public AccountController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest loginRequest)
		{
			var claims = new List<Claim>();

			User? patient = dbContext.Users
				.Where(e => e.Email == loginRequest.Email && e.Password == loginRequest.Password)
				.FirstOrDefault();

			if (patient != null)
			{
				string role = patient.Type.Equals(0) ? "Admin" : "Patient";

				claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
						new Claim(ClaimTypes.Email, patient.Email),
						new Claim(ClaimTypes.Name, patient.Name),
						new Claim(ClaimTypes.Role, role),
					};
			}
			else
			{
				ModelState.AddModelError("Password", "Username or Password wrong !");
				return View();
			}

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
										  new ClaimsPrincipal(claimsIdentity));

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterRequest request)
		{
			var patient = new User
			{
				Name = request.Name,
				Email = request.Email,
				Password = request.Password,
				Type = 1
			};

			dbContext.Users.Add(patient);
			await dbContext.SaveChangesAsync();

			return RedirectToAction("Login");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();

			return RedirectToAction("Login");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
