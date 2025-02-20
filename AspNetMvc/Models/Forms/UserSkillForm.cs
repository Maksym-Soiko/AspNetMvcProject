namespace AspNetMvc.Models.Forms;

public class UserSkillForm
{
    public Guid SkillId { get; set; }
    public int Level { get; set; }
    public bool Selected { get; set; }
}
