namespace Auth;

using System.ComponentModel.DataAnnotations;

public class UserModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; }
}

