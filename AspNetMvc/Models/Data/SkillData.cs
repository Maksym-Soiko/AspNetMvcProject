namespace AspNetMvc.Models.Data;

public static class SkillData
{
    public static readonly Dictionary<int, string> DifficultyLevels = new()
        {
            { 1, "Дуже легко" },
            { 2, "Легко" },
            { 3, "Середньо" },
            { 4, "Важко" },
            { 5, "Дуже важко" }
        };

    public static readonly List<ColorItemModel> Colors =
        [
            new ColorItemModel { Name = "Orange", HexCode = "#e44d25" },
            new ColorItemModel { Name = "Blue", HexCode = "#379ad6" },
            new ColorItemModel { Name = "Yellow", HexCode = "#fdcd00" },
            new ColorItemModel { Name = "Green", HexCode = "#40b883" },
            new ColorItemModel { Name = "Slateblue", HexCode = "#6a5acd" },
            new ColorItemModel { Name = "Navy", HexCode = "#000080" },
            new ColorItemModel { Name = "Purple", HexCode = "#800080" },
            new ColorItemModel { Name = "Cyan", HexCode = "#00ffff" },
            new ColorItemModel { Name = "Teal", HexCode = "#008080" },
            new ColorItemModel { Name = "Magenta", HexCode = "#ff00ff" }
        ];
}