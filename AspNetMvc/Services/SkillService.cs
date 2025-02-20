using AspNetMvc.Models;

namespace AspNetMvc.Services;

public class SkillService : BaseService<SkillModel>
{
    public SkillService() : base("skills.json") { }
}