using System.Net;

namespace Asclepius.Auth.Data.Exceptions;

public abstract class DataException(string message) : Exception(message)
{
    
    public abstract HttpStatusCode StatusCode { get; }
    
}