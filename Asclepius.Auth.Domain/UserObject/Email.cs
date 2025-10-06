using Asclepius.Auth.Domain.Exceptions;

namespace Asclepius.Auth.Domain.UserObject;

public record Email
{
    public Email(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new InvalidEmailException("Invalid email");
        Value = email;
    }

    [Obsolete]
    private Email()
    {
    }

    public string Value { get; }

    public virtual bool Equals(Email? other)
    {
        return other?.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }
}