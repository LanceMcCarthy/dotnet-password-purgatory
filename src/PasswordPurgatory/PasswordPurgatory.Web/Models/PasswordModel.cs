using System.ComponentModel.DataAnnotations;

namespace PasswordPurgatory.Web.Models;

public class PasswordModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
