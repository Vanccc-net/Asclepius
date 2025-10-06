using Asclepius.DTO.ProfileObject;

namespace Asclepius.DTO;

public class Profile
{
    public Profile(string id, DateOnly birthDate, Gender gender, BloodType bloodType)
    {
        Id = id;
        BirthDate = birthDate;
        Gender = gender;
        BloodType = bloodType;
    }

    public string Id { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public BloodType BloodType { get; set; }
}