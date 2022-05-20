namespace Rogue.Management.View.Model;

public class AuthenticationResult
{
    public AuthenticationResult(string token, User user)
    {
        this.Token = token;
        this.User = user;
    }

    public string Token { get; }

    public User User { get; }
}