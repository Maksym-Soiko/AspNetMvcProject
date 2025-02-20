using Microsoft.EntityFrameworkCore;

namespace AspNetMvc.Models;

public class SiteContext(DbContextOptions<SiteContext> options) : DbContext(options)
{
    public virtual DbSet<UserInfoModel> UserInfos { get; set; }
    public virtual DbSet<SkillModel> Skills { get; set; }
    public virtual DbSet<UserSkillModel> UserSkills { get; set; }
}
