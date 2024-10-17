using System.ComponentModel.DataAnnotations;

namespace pizzalandClient.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}