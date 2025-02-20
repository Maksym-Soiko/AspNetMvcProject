namespace AspNetMvc.Models;

public class UserSkillModel
{
    public Guid Id { get; set; }

    public virtual UserInfoModel UserInfo { get; set; } = null!;

    public virtual SkillModel Skill { get; set; } = null!;

    public int Level { get; set; }
}