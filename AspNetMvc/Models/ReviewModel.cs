using System.ComponentModel.DataAnnotations;

namespace AspNetMvc.Models;

public class ReviewModel
{
    public ReviewModel()
    {
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; set; }

    [MaxLength(500)]
    public string Text { get; set; } = null!;

    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual UserInfoModel UserInfo { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
