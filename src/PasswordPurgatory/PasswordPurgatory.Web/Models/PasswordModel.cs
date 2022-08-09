using System.ComponentModel.DataAnnotations;

namespace PasswordPurgatory.Web.Models;

public class PasswordModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
    public string ConfirmPassword { get; set; }
}
