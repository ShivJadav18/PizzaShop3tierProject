using PizzaShop.Repository.Data;
using PizzaShop.Repository.ViewModels;

namespace PizzaShop.Service.Interfaces;

public interface IMenuService{
    

    public void AddCategoryService(Category category);

    public List<Category> GetCategoriesService();

    public Category GetCategoryById(int categoryid);

    public void EditCategoryService(int id,string name,string description);

    public void DeleteCategoryService(int id);

    public List<Item> GetItemsService(int categoryid);

    public ItemsandCategories GetItemsandCategoriesService(int categoryid);
}