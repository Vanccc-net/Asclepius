using System.Security.Claims;

namespace Asclepius.Auth.Domain;

public record RefreshToken
{
    //TODO: Убрать Claims
    private readonly List<Claim> _claims = new();

    private RefreshToken(string userId, List<Claim> claims)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        UserId = userId;
        _claims = claims ?? throw new ArgumentNullException(nameof(claims));
        Value = Guid.NewGuid().ToString();
    }

    public string Value { get; }
    public string UserId { get; }
    public IReadOnlyCollection<Claim> Claims => _claims.AsReadOnly();

    public static RefreshToken Create(string userId, List<Claim> claims)
    {
        return new RefreshToken(userId, claims);
    }
}