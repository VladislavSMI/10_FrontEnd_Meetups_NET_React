using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
  public class RegisterDto
  {
    [Required]
    public string DisplayName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}", ErrorMessage = "Password must be between 4 to 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.")]
    public string Password { get; set; }
    [Required]
    public string UserName { get; set; }

  }
}