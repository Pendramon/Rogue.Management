namespace Rogue.Management.View.Model.Interfaces;

public interface IResponse<out T>
{
    public T? Result { get; }

    public bool Succeeded { get; }

    public IEnumerable<IError> Errors { get; }
}