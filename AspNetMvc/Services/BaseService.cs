using System.Text.Encodings.Web;
using System.Text.Json;

namespace AspNetMvc.Services;

public abstract class BaseService<TModel> where TModel : class
{
    protected readonly string DataFile;
    protected List<TModel> Items { get; set; } = [];

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    protected BaseService(string dataFile)
    {
        DataFile = dataFile;
        Load();
    }

    public void Load()
    {
        if (File.Exists(DataFile))
        {
            Items = JsonSerializer.Deserialize<List<TModel>>(File.ReadAllText(DataFile), _jsonOptions) ?? [];
        }
    }

    public List<TModel> GetAll()
    {
        return Items;
    }

    public void Add(TModel model)
    {
        Items.Add(model);
        SaveChanges();
    }

    public TModel? FindById(Guid id)
    {
        return Items.FirstOrDefault(item => (item as dynamic).Id == id);
    }

    public void SaveChanges()
    {
        File.WriteAllText(DataFile, JsonSerializer.Serialize(Items, _jsonOptions));
    }

    public void Delete(TModel model)
    {
        Items.Remove(model);
    }

    public void Delete(Guid id)
    {
        var itemToRemove = FindById(id);
        if (itemToRemove != null)
        {
            Items.Remove(itemToRemove);
            SaveChanges();
        }
    }
}