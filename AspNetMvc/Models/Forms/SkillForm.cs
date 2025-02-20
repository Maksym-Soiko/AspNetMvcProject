using AspNetMvc.Models.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetMvc.Models.Forms;

public class SkillForm
{
    public SkillForm() { }

    public SkillForm(SkillModel model)
    {
        Title = model.Title;
        Description = model.Description;
        Color = model.Color;
        Complexity = model.Complexity;
        Logo = model.Logo;
    }

    public void Update(SkillModel model)
    {
        model.Title = Title;
        model.Description = Description;
        model.Color = Color;
        model.Complexity = Complexity;
        model.Logo = Logo;
    }

    [DisplayName("Назва")]
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [MaxLength(40, ErrorMessage = "Максимум 40 символів")]
    public string Title { get; set; } = null!;

    [DisplayName("Опис")]
    public string? Description { get; set; }

    [DisplayName("Колір")]
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    public string Color { get; set; } = null!;

    [DisplayName("Складність")]
    [Required(ErrorMessage = "Будь ласка виберіть рівень складності")]
    public string Complexity { get; set; } = null!;

    [DisplayName("Логотип")]
    [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Тільки файли з розширенням .jpg, .jpeg або .png дозволені.")]
    public string? Logo { get; set; }

    public static Dictionary<int, string> DifficultyLevels => SkillData.DifficultyLevels;

    public List<ColorItemModel> Colors => SkillData.Colors;
}