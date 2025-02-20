using System.ComponentModel.DataAnnotations;

namespace AspNetMvc.Models;

public class UserInfoModel
{
    public UserInfoModel()
    {
        MainImage = string.Empty;
    }

    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public DateTime BirthDate { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string University { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Specialty { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    public string? Profession { get; set; }

    public string MainImage { get; set; } = string.Empty; 

    public virtual ICollection<string>? Images { get; set; } = new List<string>();

    public virtual ICollection<UserSkillModel> UserSkills { get; set; } = new List<UserSkillModel>();
}
