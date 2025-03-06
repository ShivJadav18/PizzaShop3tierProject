using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using PizzaShop.Repository.Data;
using PizzaShop.Repository.ViewModels;
using PizzaShop.Service.Interfaces;

namespace PizzaShop3tierProject.Controllers;

public class MenuModuleController : Controller{

    private readonly IMenuService _menuservice;

    public MenuModuleController(IMenuService menuservice){
        _menuservice = menuservice;
    }

    [Authorize]
    public IActionResult ItemAndModifier(){
       var categoriesanditem = GetItemsandCategories();
        return View(categoriesanditem);
    }

    [Authorize]
    public IActionResult EditItem(int itemid){

        Item item = _menuservice.GetItemByIdService(itemid);

        if(string.IsNullOrEmpty(item.Name)){
            TempData["error"] = "There is some internal error.";
            return Json(new { success  = false});
        }
        
        return Json(new { success = true, 
        Name = item.Name ,Imageurl = item.Imageurl,Description = item.Description, UnitId = item.UnitId, Itemtype = item.Itemtype, Rate = item.Rate, 
        Quantity = item.Quantity, Defaulttax = item.Defaulttax
        , Taxpercentage = item.Taxpercentage, Isavailable = item.Isavailable, CategoryId = item.CategoryId, Shortcode = item.Shortcode });
    }

    [Authorize]
    [HttpPost]

    public IActionResult EditItem(NewItem editeditem){
        
        string token = Request.Cookies["jwtCookie"];
        var userid = GetClaimValueHelper(token,"Userid");
        editeditem.Updatedby = Convert.ToInt32(userid);

        Message message = _menuservice.UpdateItemService(editeditem);

        if(message.error){
            TempData["error"] = message.errorMessage;
            return Json(new {success = false, message = message.errorMessage});
        }
        TempData["success"] = "Item is successfully updated.";
        return Json(new {success = true});

    }

    [Authorize]
    [HttpPost]
    public IActionResult AddNewItemAction(NewItem newItem){

        string token = Request.Cookies["jwtCookie"];
        var userid = GetClaimValueHelper(token,"Userid");
        newItem.Createdby = Convert.ToInt32(userid);
        newItem.Updatedby = Convert.ToInt32(userid);
        Message message = _menuservice.AddNewItemService(newItem);

        if(message.error){
            TempData["error"] = message.errorMessage;
            return Json(new{success = false});
        }
        TempData["success"] = "New Item is successfully added";
       return Json(new{success = true});
    }
   
    public IActionResult ItemsForpagination(Items itemsmodel){
        Items itemobj = _menuservice.GetItemsModel(itemsmodel);

        return PartialView("_Items",itemobj);
    }

    [Authorize]
    public IActionResult ForItems(){
        var categoriesanditem = GetItemsandCategories();
        return PartialView("_ItemPartialView",categoriesanditem);
    }

    [Authorize]
    public IActionResult ForModifiers(){
        return PartialView("_ModifierPartialView");
    }

    [Authorize]
    [HttpPost]
    public IActionResult AddCategory(Category category){
        var token = Request.Cookies["jwtCookie"];
        var userid = GetClaimValueHelper(token,"Userid");
        category.Createdby = Convert.ToInt32(userid);
        category.Updatedby = Convert.ToInt32(userid);
        _menuservice.AddCategoryService(category);
        return Json(new { success = true , message = "New Category is successfully created!"});
    }

    [Authorize]
    public IActionResult EditCategory(int categoryid){

        var categories = _menuservice.GetCategoriesService();

        var category = _menuservice.GetCategoryById(categoryid);
        TempData["categoryid"] = category.CategoryId;
        return Json(new{success = true , categoryid = category.CategoryId, name = category.Categoryname , description = category.Description});

    }

    
    [Authorize]
    [HttpPost]

    public IActionResult EditCategory(string categoryname, string categorydescription){
        int id = Convert.ToInt32(TempData["categoryid"]);
        _menuservice.EditCategoryService(id,categoryname,categorydescription);
        return Json(new {success = true , message = "Category Updated Successfully!"});
    }


    [Authorize]

    public IActionResult DeleteCategory(int id){

        _menuservice.DeleteCategoryService(id);

        return Json(new {success = true, message = "Category successfully deleted."});
    }

    private string GetClaimValueHelper(string token, string claimType){
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = tokenHandler.ReadJwtToken(token);

        var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);

        return claim.Value;
    }

    private List<Category> GetCategories(){
        var categories = _menuservice.GetCategoriesService();
        return categories;
    }

    public ItemsandCategories GetItemsandCategories(int categoryid = 0,string searchval = "",int count = 5, int pageno = 1){
        var ItemsandCategories = _menuservice.GetItemsandCategoriesService(categoryid,searchval,count,pageno);
        return ItemsandCategories;
    }


}