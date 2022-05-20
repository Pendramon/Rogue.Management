namespace Rogue.Management.View.Model.Interfaces;

public interface IError
{
    public string? Field { get; }

    public string Message { get; }
}