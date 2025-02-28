using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class Menu : IMenu{

    private readonly PizzaShopDbContext _context;

    public Menu(PizzaShopDbContext context){
        _context = context;
    }

}