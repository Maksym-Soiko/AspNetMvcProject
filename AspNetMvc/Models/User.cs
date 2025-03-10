using Microsoft.AspNetCore.Identity;

namespace AspNetMvc.Models;

public class User : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public string? ProfileImage { get; set; }

    public virtual ICollection<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();
}