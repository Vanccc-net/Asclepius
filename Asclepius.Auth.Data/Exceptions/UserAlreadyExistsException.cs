using System.Net;

namespace Asclepius.Auth.Data.Exceptions;

public class UserAlreadyExistsException(string message) : DataException(message)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.Conflict;
}