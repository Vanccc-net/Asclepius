using System.Net;

namespace Asclepius.Auth.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message)
{
    public abstract HttpStatusCode StatusCode { get; }
}