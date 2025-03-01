namespace AspNetMvc.Areas.Auth.Models.Forms;

public class ChangePasswordForm
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}