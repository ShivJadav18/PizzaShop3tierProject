using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class Menu : IMenu{

    private readonly PizzaShopDbContext _context;

    public Menu(PizzaShopDbContext context){
        _context = context;
    }

    public void AddCategory(Category category){
        _context.Add(category);
        _context.SaveChanges();
    }

    public List<Category> GetCategories(){
        var categories = _context.Categories.Where(c => c.Isdeleted == false).OrderBy(c => c.CategoryId).ToList();

        return categories;
    }

    public void UpdateCategory(Category category){
        _context.Update(category);
        _context.SaveChanges();
    }

    public void RemoveCategory(Category category){
       category.Isdeleted = true;
        _context.SaveChanges();
    }
}