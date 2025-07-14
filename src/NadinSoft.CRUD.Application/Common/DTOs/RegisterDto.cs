using System.ComponentModel.DataAnnotations;

namespace NadinSoft.CRUD.Application.Common.DTOs;

public class RegisterDto
{    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;
}