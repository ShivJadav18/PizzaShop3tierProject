using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MailKit.Net.Smtp;
using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Repository.ViewModels;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementation;

public class Authenticate : IAuthenticate{

    private readonly IUser _repouser;

    public Authenticate (IUser repouser){
        _repouser = repouser;
    }

    public string GetAuthenticate(Userlogin user){
        User userobj = _repouser.GetUser(user);

        if(userobj.Email == null || !BCrypt.Net.BCrypt.Verify( user.Password,userobj.Password)){
            return "";
        }
       
        var role = userobj.Role.Name;
        var claims = new[] {
                new Claim(ClaimTypes.Role , role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserName",userobj.Username),
                new Claim("Userid",userobj.UserId.ToString())
            };

        var token = GenerateJSONWebToken(claims);

        return token;
    }

    public bool SendEmailForResetpass( string email,string baseUrl){
        Userlogin usertempobj = new Userlogin{Email = email}; 

        User user = _repouser.GetUser(usertempobj);

        if(user.Email == null){
            return false;
        }

        EmailSendingHelper(email,baseUrl);
        return true;
    }

    public bool ResetPassService(string email,string password){

        Userlogin usertempobj = new Userlogin{Email = email,Password = BCrypt.Net.BCrypt.HashPassword(password)}; 

        User userobj = _repouser.SetPass(usertempobj);

        if(userobj.Email == null){
            return false;
        }

        return true;
    }


    private async void EmailSendingHelper(string email,string baseUrl){
         // making of mail
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MailboxAddress("Pizza Shop", "test.dotnet@etatvasoft.com"));
            message.To.Add(new MailboxAddress("PizzaShop User", email));
            message.Subject = "Reset your password ";

            // making of url link
           
            var bodyBuilder = new BodyBuilder();
            var url = $"{baseUrl}";

            bodyBuilder.HtmlBody = $"Reset your password <a href='{url}/Authenticate/Resetpage'>Here </a>";


            message.Body = bodyBuilder.ToMessageBody();

            //sending of mail 
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("mail.etatvasoft.com", 587, false);
                await client.AuthenticateAsync("test.dotnet@etatvasoft.com", "P}N^{z-]7Ilp");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        return ;
    }
     private string GenerateJSONWebToken(Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyNameis_Shiv_Jadav_018_Tatvasoft_007_James_Bond"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
 
            var token = new JwtSecurityToken(
                issuer: "Pizzashop App",
                audience: "dotnetclient",
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials,
                claims: claims
                );
 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    
}