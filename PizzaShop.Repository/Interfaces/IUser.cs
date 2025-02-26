using PizzaShop.Repository.Data;
using PizzaShop.Repository.ViewModels;
 
 namespace PizzaShop.Repository.Interfaces;

 public interface IUser{

    public User GetUser(Userlogin user);

    public User SetPass(Userlogin user);

    public User UpdateUser(User user);

    public IQueryable<User> GetUserslist(string searchval);

    public void AddUser(User userobj);

    public User GetUserById(int id);

     public void EditUser(Usertemp user);

     public void DeleteUser(int id);
}