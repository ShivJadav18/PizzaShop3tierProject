using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PizzaShop3tierProject.Controllers;
 [Authorize(Roles = "super admin")]
public class RoleController : Controller{

    public IActionResult Roleselect(){
        return View();
    }

    public IActionResult Permission(){
        return View();
    }

}