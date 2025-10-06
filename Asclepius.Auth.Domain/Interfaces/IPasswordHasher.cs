namespace Asclepius.Auth.Domain.Interfaces;

public interface IPasswordHasher
{
    string GeneratePasswordHash(string password);
    bool VerifyPasswordHash(string password, string hash);
}