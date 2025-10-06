using System.Text.Json.Serialization;

namespace Asclepius.DTO;

public class Notification
{
    [JsonConstructor]
    public Notification(string userId, string email, string message)
    {
        UserId = userId;
        Email = email;
        Message = message;
    }

    public string UserId { get; private set; }
    public string Email { get; private set; }
    public long TelegramId { get; private set; }
    public string Message { get; private set; }

    public static Notification Create(string userId, string email, string message)
    {
        return new Notification(userId, email, message);
    }
}