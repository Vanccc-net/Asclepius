using System.Net;

namespace Asclepius.Auth.Domain.Exceptions;

public class InvalidPasswordException(string message) : DomainException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}