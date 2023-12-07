using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Models;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Security.Claims;

namespace MvcProject.Controllers
{
	public class HomeController : Controller
	{
		[AllowAnonymous]
		public IActionResult Index()
		{
			ClaimsPrincipal user = HttpContext.User;
			if (user == null || !user.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}
			else if (user.IsInRole("Admin"))
			{
				return RedirectToAction("Index", "Admin");
			}
			else if (user.IsInRole("Patient"))
			{
				return RedirectToAction("Index", "UserAppointments");
			}
			return View();
		}

		[Authorize]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}