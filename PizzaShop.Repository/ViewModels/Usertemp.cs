using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Repository.ViewModels;

public partial class Usertemp
{

    public int UserId { get; set; }

    [Required(ErrorMessage = "Firstname is required")]
    public string Firstname { get; set; } = null!;

    [Required(ErrorMessage = "Lastname is required")]
    public string Lastname { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

     [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Contactnumber is required")]
    public string? Contactnumber { get; set; }
    
 
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "State is required")]
    public string? State { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Zipcode is required")]
    public string? Zipcode { get; set; }

 
    public string? Imageurl { get; set; }

    [Required]
    public bool? Status { get; set; }

}