using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetMvc.Areas.Auth.Models.Forms;

public class LoginForm
{
    [DisplayName("Логін")]
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти")]
    public string Login { get; set; }

    [DisplayName("Пароль")]
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    public string Password { get; set; }
}
