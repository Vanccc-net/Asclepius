using System.Net;

namespace Asclepius.Auth.Domain.Exceptions;

public class InvalidEmailException(string message) : DomainException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}