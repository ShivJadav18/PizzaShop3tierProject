using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaShop.Service.Interfaces;

namespace PizzaShop3tierProject.Controllers;

public class MenuModuleController : Controller{

    private readonly IMenuService _menuservice;

    public MenuModuleController(IMenuService menuservice){
        _menuservice = menuservice;
    }

    [Authorize]
    public IActionResult ItemAndModifier(){

        return View();
    }

    [Authorize]

    public IActionResult ForItems(){
        return PartialView("_ItemPartialView");
    }

    public IActionResult ForModifiers(){
        return PartialView("_ModifierPartialView");
    }

}