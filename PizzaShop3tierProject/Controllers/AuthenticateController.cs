using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Repository.ViewModels;
using PizzaShop.Service.Implementation;
using PizzaShop.Service.Interfaces;
using PizzaShop3tierProject.Models;

namespace PizzaShop3tierProject.Controllers;

public class AuthenticateController : Controller
{
    private readonly ILogger<AuthenticateController> _logger;
    private readonly IAuthenticate _userauth;

    public AuthenticateController(ILogger<AuthenticateController> logger, IAuthenticate userauth)
    {
        _logger = logger;
        _userauth = userauth;
    }

    public IActionResult Login()
    {
        var token = Request.Cookies["jwtCookie"];
        if (token != null)
        {
            return RedirectToAction("Dashboardpage", "Dashboard");
        }

        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Login(Userlogin user)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var jwtToken = _userauth.GetAuthenticate(user);
        if (jwtToken == "")
        {
            TempData["error"] = "Email or Password is Wrong.";
            return View();
        }
        if (user.rememberme)
        {
            SetJWTCookie(jwtToken, 7, "jwtCookie");
        }
        else
        {
            SetJWTCookie(jwtToken, 1, "jwtCookie");
        }
        TempData["success"] = "You are Successfully Logged In!";
        return RedirectToAction("Dashboardpage", "Dashboard");
    }

    public IActionResult Forgotpage()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Forgotpage(string email)
    {
        // storing the email id in tempdata.
        TempData["user_email"] = email;

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        bool result = _userauth.SendEmailForResetpass(email, baseUrl);
        if (result)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = true, message = "Please Enter Right Email!" });
        }
    }

    public async Task<IActionResult> Resetpage()
    {

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Resetpage(string password)
    {

        // Fetching email id from temp data
        string email = TempData["user_email"].ToString();

        bool result = _userauth.ResetPassService(email, password);

        if (result)
        {
            return Json(new { success = true, message = "User's Password is successfully changed." });
        }
        else
        {
            return Json(new { success = false, message = "User's Password is Not Changed!" });
        }

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

    private void SetJWTCookie(string token, int days, string name)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(days),
        };
        Response.Cookies.Append(name, token, cookieOptions);
    }
}
