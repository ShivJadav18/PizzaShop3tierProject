using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Repository.ViewModels;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementation;

public class MenuService : IMenuService{
    
    private readonly IMenu _menu;

    public MenuService(IMenu menu){
        _menu = menu;
    }

    public void AddCategoryService(Category category){

        _menu.AddCategory(category);

    }

    public List<Category> GetCategoriesService(){
        var categories = _menu.GetCategories();
        var ItemsandCategories = new ItemsandCategories{
            categories = categories
        };
        return categories;
    }

    public Category GetCategoryById(int categoryid){
        var categories = _menu.GetCategories();

        var category = categories.FirstOrDefault(c => c.CategoryId == categoryid);

        return category;
    }

    public void DeleteCategoryService(int id){
        var categories = _menu.GetCategories();

        var category = categories.FirstOrDefault(c => c.CategoryId == id);

        _menu.RemoveCategory(category);        
    }

    public void EditCategoryService(int id,string name,string description){

        var categories = _menu.GetCategories();

        var category = categories.FirstOrDefault(c => c.CategoryId == id);

        category.Categoryname = name;
        category.Description = description;

        _menu.UpdateCategory(category);
        
    }
}