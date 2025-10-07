using System.Net;

namespace Asclepius.Auth.Data.Exceptions;

public class RoleNotFoundException(string message) : DataException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}