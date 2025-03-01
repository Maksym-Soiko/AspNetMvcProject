namespace AspNetMvc.Services;

public class FileStorageService(IWebHostEnvironment environment)
{
    public async Task<string> SaveFileAsync(IFormFile file, string basePath, bool useSubdirectories = false)
    {
        var extension = Path.GetExtension(file.FileName);
        var filename = Guid.NewGuid().ToString() + extension;

        string fullPath;
        string relativePath;

        if (useSubdirectories)
        {
            var dir1 = filename[0].ToString();
            var dir2 = filename[1].ToString();
            Directory.CreateDirectory(Path.Combine(environment.WebRootPath, basePath, dir1, dir2));
            fullPath = Path.Combine(environment.WebRootPath, basePath, dir1, dir2, filename);
            relativePath = Path.Combine(dir1, dir2, filename);
        }
        else
        {
            Directory.CreateDirectory(Path.Combine(environment.WebRootPath, basePath));
            fullPath = Path.Combine(environment.WebRootPath, basePath, filename);
            relativePath = filename;
        }

        using var fs = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(fs);

        return Path.Combine(basePath, relativePath).Replace("\\", "/");
    }

    public void DeleteFile(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            Console.WriteLine("Помилка: Шлях до файлу порожній.");
            return;
        }

        if (string.IsNullOrEmpty(environment.WebRootPath))
        {
            throw new InvalidOperationException("WebRootPath не ініціалізований.");
        }

        var fullFilename = Path.Combine(environment.WebRootPath, relativePath.Trim());
        Console.WriteLine($"Шлях до файлу: {fullFilename}");

        if (File.Exists(fullFilename))
        {
            File.Delete(fullFilename);
            Console.WriteLine("Файл видалено.");
        }
        else
        {
            Console.WriteLine("Файл не знайдено.");
        }
    }

    public async Task<List<string>> UploadUserPhotosAsync(IFormFile[] photos)
    {
        var uploadedImages = new List<string>();

        foreach (var photo in photos)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            var filePath = Path.Combine("wwwroot/user_info_uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            uploadedImages.Add(fileName);
        }

        return uploadedImages;
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
        {
            return string.Empty;
        }

        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var path = Path.Combine(folder, fileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"uploads/{fileName}";
    }
}