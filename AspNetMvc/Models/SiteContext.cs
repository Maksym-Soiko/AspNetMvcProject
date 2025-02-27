using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetMvc.Models;

public class SiteContext(DbContextOptions options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public virtual DbSet<UserInfoModel> UserInfos { get; set; }
    public virtual DbSet<SkillModel> Skills { get; set; }
    public virtual DbSet<UserSkillModel> UserSkills { get; set; }
}
