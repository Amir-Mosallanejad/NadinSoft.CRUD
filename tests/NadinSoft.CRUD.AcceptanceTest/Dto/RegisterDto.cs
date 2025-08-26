namespace NadinSoft.CRUD.AcceptanceTest.Dto;

/// <summary>
/// Represents the data transfer object used for registering a new user.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> containing the user's email.
    /// </value>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> containing the user's password.
    /// </value>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets the confirmation of the password.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> containing the user's password confirmation.
    /// </value>
    public string ConfirmPassword { get; set; } = null!;
}