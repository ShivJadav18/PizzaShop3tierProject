using PizzaShop.Repository.Data;
using PizzaShop.Repository.ViewModels;

namespace PizzaShop.Service.Interfaces;

public interface IUserservice{

    public User GetProfileService(string email);

    public User UpdateProfileService(User user);

    public Userlistmodel GetUsersListService(Userlistmodel userslist);

    public void AddUserService(User userobj,string email);

    public Usertemp GetUsertemp(int id);

    public void EditUserService(Usertemp usertemp);

    public void DeleteUserService(int id);
}