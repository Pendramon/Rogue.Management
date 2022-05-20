using Rogue.Management.View.Model.Interfaces;

namespace Rogue.Management.View.Model;

public class Response<T> : IResponse<T>
{
    public T? Result { get; set; }

    public bool Succeeded { get; set; }

    public IEnumerable<IError> Errors { get; set; } = Enumerable.Empty<IError>();
}
