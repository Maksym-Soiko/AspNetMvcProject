using System.ComponentModel.DataAnnotations;

namespace AspNetMvc.Models;

public class ColorItemModel
{
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public string HexCode { get; set; } = null!;
}
