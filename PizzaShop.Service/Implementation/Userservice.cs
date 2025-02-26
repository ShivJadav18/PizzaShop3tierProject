
using Org.BouncyCastle.Asn1.Ocsp;
using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Repository.ViewModels;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementation;


public class Userservice : IUserservice{

    private readonly IUser _repouser;

    public Userservice (IUser repouser){
        _repouser = repouser;
    }

    public User GetProfileService(string email){
        var usertemp = new Userlogin{Email = email};
        User userobj = _repouser.GetUser(usertemp);

        if(userobj.Email == null){
        return new User{};
        }

        return userobj;
    }

    public Usertemp GetUsertemp(int id){
        User userobj = _repouser.GetUserById(id);

        Usertemp usertemp = new Usertemp{
                Email = userobj.Email,
                UserId = userobj.UserId,
                Firstname = userobj.Firstname,
                Lastname = userobj.Lastname,
                Username = userobj.Username,
                Country = userobj.Country,
                State = userobj.State,
                City = userobj.City,
                Status = userobj.Status,
                Address = userobj.Address,
                Contactnumber = userobj.Contactnumber,
                Zipcode = userobj.Zipcode,
                RoleId = userobj.RoleId,
                Imageurl = userobj.Imageurl
        };

        return usertemp;
    }

    public void EditUserService(Usertemp usertemp){
        _repouser.EditUser(usertemp);
    }

    public Userlistmodel GetUsersListService( Userlistmodel userslistobj){
        var totaluserslist = _repouser.GetUserslist(userslistobj.searchval);
        var totalUsers = totaluserslist.Count();
        var userslist = totaluserslist.Skip((userslistobj.pageno-1)* userslistobj.count).Take(userslistobj.count).ToList();

        var Userlistmodel = new Userlistmodel{
                totalUsers =totalUsers,
                Userslist = userslist,                              
                count = userslistobj.count,
                pageno = userslistobj.pageno
            };

        return Userlistmodel;
    }

    public void DeleteUserService(int id){
        _repouser.DeleteUser(id);
    }

    public void AddUserService(User userobj,string email){
        Userlogin usetemp = new Userlogin{Email = email};
        User addinguser = _repouser.GetUser(usetemp);
        userobj.Password = BCrypt.Net.BCrypt.HashPassword(userobj.Password);
        userobj.Createdby = addinguser.UserId;
        userobj.Updatedby = addinguser.UserId;

        _repouser.AddUser(userobj);
    }

    public User UpdateProfileService(User usertemp){

        User user = _repouser.UpdateUser(usertemp);

        if(user.Email == null){
        return new User{};
        }

        return user;
    }
}