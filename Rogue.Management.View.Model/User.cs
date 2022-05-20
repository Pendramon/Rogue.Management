namespace Rogue.Management.View.Model;

/// <summary>
/// An application user.
/// </summary>
public class User
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="createdAt">The date and time of the user registration.</param>
    public User(string username, string email, DateTime createdAt)
    {
        this.Username = username;
        this.Email = email;
        this.CreatedAt = createdAt;
    }

    /// <summary>
    /// Gets the user's username.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the user's date and time of registering.
    /// </summary>
    public DateTime CreatedAt { get; }
}