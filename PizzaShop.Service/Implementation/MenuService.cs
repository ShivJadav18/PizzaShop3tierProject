using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementation;

public class MenuService : IMenuService{
    
    private readonly IMenu _menu;

    public MenuService(IMenu menu){
        _menu = menu;
    }

}