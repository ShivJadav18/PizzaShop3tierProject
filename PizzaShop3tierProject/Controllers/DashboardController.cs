
using PizzaShop.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace PizzaShop3tierProject.Controllers{

    public class DashboardController : Controller {

        private readonly PizzaShopDbContext _context;

        public DashboardController(PizzaShopDbContext context){

            _context = context;

        }

        public async Task<IActionResult> Dashboardpage(){

            var token = Request.Cookies["jwtCookie"];
            if(token != null){
                return View();
            }

            return RedirectToAction("Login","Authenticate");

        }

    }

}