using System.Net;

namespace Asclepius.Auth.Data.Exceptions;

public class UserNotFoundException(string message) : DataException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}