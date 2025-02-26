using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Repository.Data;
using PizzaShop.Repository.ViewModels;
using PizzaShop.Service.Interfaces;

namespace PizzaShop3tierProject.Controllers;

[Authorize]
public class UserController:Controller{

    private readonly IUserservice _userservice;
    private readonly IAuthenticate _authenticate;

    public UserController(IUserservice userservice,IAuthenticate authenticate){
        _authenticate = authenticate;
        _userservice = userservice;
    }

    public IActionResult UserslistafterFirstTime(Userlistmodel userlistmodel){
        
        Userlistmodel userslist = _userservice.GetUsersListService(userlistmodel);

        return PartialView("_UserTable",userslist);

    }

    public IActionResult Userslist(){
         Userlistmodel userstemp = new Userlistmodel{
            pageno = 1,
            count = 2,   
        };
        Userlistmodel userslist = _userservice.GetUsersListService(userstemp);

        return View(userslist);
    }

    public IActionResult Adduser(){
        return View();
    }

     [Authorize(Roles = "super admin")]
    public IActionResult Edit(int id){

        Usertemp usertemp = _userservice.GetUsertemp(id);

        return View(usertemp);
    }

    [HttpPost]
     [Authorize(Roles = "super admin")]
    public IActionResult Edit(Usertemp usertemp){
        _userservice.EditUserService(usertemp);

       return RedirectToAction("Userslist","User");
    }
    
     [Authorize(Roles = "super admin")]
    public IActionResult Delete(int id){
        if (id == null)
        {
            return Json(new{ success = false , message = "User not found."});
        }

        _userservice.DeleteUserService(id);

        return Json(new { success = true, message = "User is successfully deleted!"});
    }

    [HttpPost]
     [Authorize(Roles = "super admin")]
    public IActionResult Adduser(User userobj){
        var token = Request.Cookies["jwtCookie"];
        var email = GetClaimValueHelper(token,ClaimTypes.Email);

        TempData["user_email"] = userobj.Email;

        _userservice.AddUserService(userobj,email);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        bool result = _authenticate.SendEmailForResetpass(userobj.Email,baseUrl);

        return RedirectToAction("Userslist","User");
    }

    public IActionResult MyProfile(){
        var token = Request.Cookies["jwtCookie"];
        var email = GetClaimValueHelper(token,ClaimTypes.Email);
        User user = _userservice.GetProfileService(email);
        return View(user);
    }

    [HttpPost]

    public IActionResult MyProfile(User user){

        User userobj = _userservice.UpdateProfileService(user);

        return View(userobj);
    }

    public IActionResult Changepass(){
        return View();
    }


    [HttpPost]
    public IActionResult Changepass(string Currentpass,string Cnfpass,string Newpass){
         var token = Request.Cookies["jwtCookie"];
        var email = GetClaimValueHelper(token,ClaimTypes.Email);

        bool result = _authenticate.ResetPassService(email,Newpass);

        if(result){
            return Json(new{success = true, message = "user's password successfully changed!"});
        }

        return Json(new{success = false, message = "User's password does't changed because of internal error!"});
    }

    [Authorize]
    public IActionResult Logout(){

         if(Request.Cookies["jwtCookie"] != null){
             Response.Cookies.Delete("jwtCookie", new CookieOptions{
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
             });
            }

        return Json( new { success = true });
    }

    

    private string GetClaimValueHelper(string token, string claimType){
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = tokenHandler.ReadJwtToken(token);

        var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);

        return claim.Value;
    }

}