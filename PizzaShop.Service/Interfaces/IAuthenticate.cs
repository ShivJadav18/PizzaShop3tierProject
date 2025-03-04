using Microsoft.AspNetCore.Mvc;
using PizzaShop.Repository.ViewModels;
using PizzaShop.Service.Implementation;

namespace PizzaShop.Service.Interfaces;

public interface IAuthenticate{

    public string GetAuthenticate(Userlogin user);

    public  bool SendEmailForResetpass(string email,string baseUrl);

    public bool ResetPassService(string email,string password,string Currentpass="");

    // public string GetValueFromToken(string token, string claimType);
}