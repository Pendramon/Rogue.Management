namespace Rogue.Management.View.Model;

public record LoginViewModel(string UsernameOrEmail, string Password)
{
    public string? ReturnUrl { get; set; }
}
