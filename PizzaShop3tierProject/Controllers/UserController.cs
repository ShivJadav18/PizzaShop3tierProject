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

        return View(new NewUserModel{});
    }

     [Authorize(Roles = "super admin")]
    public IActionResult Edit(int id){
        var token = Request.Cookies["jwtCookie"];
        var userid = Convert.ToInt32(GetClaimValueHelper(token,"Userid"));

        if(userid == id){
            TempData["error"] = "You can not change your own details from here.";
            return RedirectToAction("Userslist","User");
        }

        Usertemp usertemp = _userservice.GetUsertemp(id);

        return View(usertemp);
    }

    [HttpPost]
     [Authorize(Roles = "super admin")]
    public IActionResult Edit(Usertemp usertemp){

        if(!ModelState.IsValid){
            return View(usertemp);
        }

        var token = Request.Cookies["jwtCookie"];
        var id = GetClaimValueHelper(token,"Userid");

        usertemp.Updatedby = Convert.ToInt32(id);

       bool result =  _userservice.EditUserService(usertemp);

       if(result){
        TempData["success"] ="User detail is updated.";
       return RedirectToAction("Userslist","User");
       }
       TempData["error"] = "There is some internal error.";
       return View(usertemp);

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
    public IActionResult Adduser(NewUserModel userobj){

        if(!ModelState.IsValid){
            return View(userobj);
        }

        var token = Request.Cookies["jwtCookie"];
        var email = GetClaimValueHelper(token,ClaimTypes.Email);

        TempData["user_email"] = userobj.Email;

       bool response = _userservice.AddUserService(userobj,email);

       if(!response){
        TempData["error"] = "There is some internal error.";
        return View();
       }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        bool result = _authenticate.SendEmailForResetpass(userobj.Email,baseUrl);

        if(!result){
            TempData["error"] = "There is an some error in email sending.";
            return RedirectToAction("Userslist","User");
        }
        TempData["success"] = "New User is successfully added.";
        return RedirectToAction("Userslist","User");
    }

    public IActionResult MyProfile(){
        var token = Request.Cookies["jwtCookie"];
        var email = GetClaimValueHelper(token,ClaimTypes.Email);
        ProfileViewModel user = _userservice.GetProfileService(email);
        return View(user);
    }

    [HttpPost]

    public IActionResult MyProfile(ProfileViewModel user){
         string token = Request.Cookies["jwtCookie"];
         var role = GetClaimValueHelper(token,ClaimTypes.Role);
         var id = GetClaimValueHelper(token,"Userid");
         user.Updatedby = Convert.ToInt32(id);
         user.Rolename = role;
        if(!ModelState.IsValid){
            return View(user);
        }

       
        var email = GetClaimValueHelper(token,ClaimTypes.Email);

        user.Email = email;

        ProfileViewModel userobj = _userservice.UpdateProfileService(user);

        if(userobj.Username == "repeated"){
            TempData["error"] = "This Username can not be used.";
            userobj.Username = "";
            return View(userobj);
        }

        if(userobj.Username == null){
            TempData["error"] = "There is some error in updating the user profile.Please Try again.";
            return RedirectToAction("Dashboardpage","Dashboard");
        }
        TempData["success"] = "User's profile is successfully updated.";
        return View(userobj);
    }

    public IActionResult Changepass(){
        return View();
    }


    [HttpPost]
    public IActionResult Changepass(string Currentpass,string Cnfpass,string Newpass){

        if(Currentpass == Newpass){
            TempData["error"] = "New password and current password can not be same.";
            return Json(new{success = false});
        }

        if(Newpass != Cnfpass){
            TempData["error"] = "Confirm password and new password are nor matching.";
            return Json(new{success = false});
        }

         var token = Request.Cookies["jwtCookie"];
        var email = GetClaimValueHelper(token,ClaimTypes.Email);

        bool result = _authenticate.ResetPassService(email,Newpass,Currentpass);

        if(result){
            TempData["success"] = "User's password successfully changed.";
            return Json(new{success = true});
        }
        TempData["error"] = "User's Entered current password is wrong.";
        return Json(new{success = false});
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