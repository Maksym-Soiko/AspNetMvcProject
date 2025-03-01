namespace AspNetMvc.Areas.Auth.Models.Forms;

public class ProfileForm
{
    public IFormFile? ProfileImage { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
}