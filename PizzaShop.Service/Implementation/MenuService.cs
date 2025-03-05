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
        return categories;
    }

    public Items GetItemsModel(Items items){
        var totalitemslist = _menu.GetItems(items.categoryid,items.searchval);
        var totalitems = totalitemslist.Count();

        var itemslist =  totalitemslist.Skip((items.pageno-1)* items.count).Take(items.count).ToList();

        var ItemsObj = new Items{
            totalitems = totalitems,
            items = itemslist,
            count = items.count,
            pageno = items.pageno,
            categoryid = items.categoryid,
            searchval = items.searchval
        };

        return ItemsObj;
    }
    
    public Message AddNewItemService(NewItem newItem){

        if(newItem.ItemImage != null){
            var fileName = Path.GetFileNameWithoutExtension(newItem.ItemImage.FileName);
                var extension = Path.GetExtension(newItem.ItemImage.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedImages");
                var path = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    newItem.ItemImage.CopyTo(fileStream);
                }

                // Save the relative path to the newItem property
                newItem.Imageurl = $"UploadedImages/{uniqueFileName}";
        }

        Item item = new Item{
            Name = newItem.Name,
            CategoryId = newItem.CategoryId,
            Description = newItem.Description,
            UnitId = newItem.UnitId,
            Imageurl = newItem.Imageurl,
            Itemtype = newItem.Itemtype,
            Rate = newItem.Rate,
            Taxpercentage = newItem.Taxpercentage,
            Defaulttax = newItem.Defaulttax,
            Quantity = newItem.Quantity,
            Shortcode = newItem.Shortcode,
            Isavailable = newItem.Isavailable,
            Createdby = newItem.Createdby,
            Updatedby = newItem.Updatedby
        };

        Message message = _menu.AddItem(item);

       
        return message;
    }

    public ItemsandCategories GetItemsandCategoriesService(int categoryid,string searchval = "",int count = 5,int pageno = 1){
       
        var categories = GetCategoriesService();
        categoryid = categories[0].CategoryId;
        var itemobj = new Items{
            categoryid=categoryid,
            searchval =searchval,
            count = count,
            pageno =pageno
        };
        var itemmodel = GetItemsModel(itemobj);
        
        ItemsandCategories ItemsandCategories = new ItemsandCategories{
            categories = categories,
            itemmodel = itemmodel
        };

        return ItemsandCategories;
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