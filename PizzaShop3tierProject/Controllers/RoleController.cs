using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Repository.Data;
using PizzaShop.Service.Interfaces;

namespace PizzaShop3tierProject.Controllers;
 [Authorize(Roles = "super admin")]
public class RoleController : Controller{

   private IRoleandPermissionServices _roleandpermissionservice;

   public RoleController(IRoleandPermissionServices roleandpermissionservice){
    _roleandpermissionservice = roleandpermissionservice;
   }

    public IActionResult Roleselect(){

        return View();
    }

    public IActionResult Permission(int id){

        Role role = _roleandpermissionservice.GetRoleByIdService(id);

        TempData["RoleName"] = role.Name;

        var permissions = _roleandpermissionservice.GetPermissionsByRoleService(role.RoleId);

        return View(permissions);
    }

[HttpPost]
    public IActionResult Permission(IEnumerable<Permission> permissions){
        string token = Request.Cookies["jwtCookie"];
        int id = Convert.ToInt32(GetClaimValueHelper(token,"Userid"));

        _roleandpermissionservice.UpdatePermissionsService(permissions,id);

        return RedirectToAction("Roleselect","Role");
    }

    private string GetClaimValueHelper(string token, string claimType){
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = tokenHandler.ReadJwtToken(token);

        var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);

        return claim.Value;
    }

}