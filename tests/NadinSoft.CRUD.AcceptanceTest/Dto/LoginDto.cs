namespace NadinSoft.CRUD.AcceptanceTest.Dto;

/// <summary>
/// Data Transfer Object (DTO) representing login credentials.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    /// <value>
    /// A valid email address required for login.
    /// </value>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    /// <value>
    /// A non-empty password required for login.
    /// </value>
    public string Password { get; set; } = null!;
}