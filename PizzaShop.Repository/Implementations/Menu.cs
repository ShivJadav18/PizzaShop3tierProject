using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Repository.ViewModels;

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

    public List<Item> GetItems(int categoryid,string searchval){
        var items = _context.Items.Where(i => (i.CategoryId == categoryid && string.IsNullOrEmpty(searchval) && i.Isdeleted == false) || (i.CategoryId == categoryid && i.Name.Contains(searchval)  && i.Isdeleted == false)).OrderBy(i => i.ItemId).ToList();
        return items;
    }

    public Message AddItem(Item item){
        try{
        _context.Add(item);
        _context.SaveChanges();
        return new Message{error = false};
        
        }catch(Exception e){
            return new Message{error = true,errorMessage = "Internal Error"};
        }

    }

    public Message UpdateItem(NewItem item){
        try{
            Item realitem = _context.Items.FirstOrDefault(i => i.ItemId == item.itemid);

            realitem.Name = item.Name;
            realitem.Description = item.Description;
            realitem.CategoryId = item.CategoryId;
            realitem.Defaulttax = item.Defaulttax;
            realitem.UnitId = item.UnitId;
            realitem.Rate = item.Rate;
            realitem.Quantity = item.Quantity;
            realitem.Taxpercentage = item.Taxpercentage;
            realitem.Itemtype = item.Itemtype;
            realitem.Imageurl = item.Imageurl;
            realitem.Isavailable = item.Isavailable;
            realitem.Shortcode = item.Shortcode;
            realitem.Updatedby = item.Updatedby;
            realitem.Updatedat = item.Updatedat;

            _context.SaveChanges();
            return new Message{error = false};
        }catch(Exception e){
        return new Message{error = true, errorMessage = "Some Internal Error."};
        }
    }

    public Item GetItem(int itemid){
        try{
        Item item = _context.Items.FirstOrDefault(i => i.ItemId == itemid);
         return item;
        }catch(Exception e){
            return new Item{};
        }
    }

}