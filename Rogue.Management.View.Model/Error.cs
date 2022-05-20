using Rogue.Management.View.Model.Interfaces;

namespace Rogue.Management.View.Model;

public class Error : IError
{
    public Error(string message, string? field = default)
    {
        this.Message = message;
        this.Field = field;
    }

    public string? Field { get; }

    public string Message { get; }
}