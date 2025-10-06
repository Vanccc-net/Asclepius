using Asclepius.Auth.Domain.Exceptions;

namespace Asclepius.Auth.Domain.UserObject;

public record Password
{
    public Password(string password, bool isHashed = false)
    {
        if (isHashed)
        {
            Hash = password;
            return;
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new InvalidPasswordException("Password must be at least 8 characters");
        Hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    [Obsolete]
    private Password()
    {
    }

    public string Hash { get; } = null!;

    public virtual bool Equals(Password? other)
    {
        return other?.Hash == Hash;
    }

    public bool Verify(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, Hash);
    }

    public override int GetHashCode()
    {
        return Hash?.GetHashCode() ?? 0;
    }
}