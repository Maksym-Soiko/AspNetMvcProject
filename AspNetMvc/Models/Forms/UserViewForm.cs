using System.ComponentModel.DataAnnotations;

namespace AspNetMvc.Models.Forms;

public class UserViewForm
{
    public UserViewForm() { }

    public UserViewForm(User model)
    {
        FullName = model.FullName;
        Email = model.Email;
        PhoneNumber = model.PhoneNumber;
        ProfileImage = model.ProfileImage;
    }

    public void Update(User model)
    {
        model.FullName = FullName;
        model.Email = Email;
        model.PhoneNumber = PhoneNumber;

        if (!string.IsNullOrEmpty(ProfileImage))
        {
            model.ProfileImage = ProfileImage;
        }
    }

    [Display(Name = "Повне ім'я")]
    [MaxLength(80, ErrorMessage = "Максимум 80 символів")]
    public string FullName { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти")]
    public string Email { get; set; }

    [Display(Name = "Номер телефону")]
    [Phone(ErrorMessage = "Некоректний номер телефону")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Фото профілю")]
    [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Тільки файли з розширенням .jpg, .jpeg або .png дозволені.")]
    public string? ProfileImage { get; set; }

    [Required(ErrorMessage = "Виберіть хоча б одну роль!")]
    public string[] SelectedRoles { get; set; }
}