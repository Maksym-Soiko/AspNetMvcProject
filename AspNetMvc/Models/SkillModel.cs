using AspNetMvc.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetMvc.Models;

public class SkillModel
{
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public string Color { get; set; }

    public string Complexity { get; set; }

    public string? Logo { get; set; }

    [NotMapped]
    public static Dictionary<int, string> DifficultyLevels => SkillData.DifficultyLevels;

    [NotMapped]
    public static List<ColorItemModel> Colors => SkillData.Colors;
}