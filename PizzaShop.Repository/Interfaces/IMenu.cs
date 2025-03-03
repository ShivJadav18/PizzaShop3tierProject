using PizzaShop.Repository.Data;

namespace PizzaShop.Repository.Interfaces;

public interface IMenu{
    public void AddCategory(Category category);

    public List<Category> GetCategories();

     public void UpdateCategory(Category category);

     public void RemoveCategory(Category category);
}