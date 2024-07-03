using Microsoft.AspNetCore.Http;
using Server.Application.Interfaces;

namespace Server.Application.Services;

public class FileService : IFileSerice
{
    private readonly string _imagePath;

    public FileService()
    {
        _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Images");
        if (!Directory.Exists(_imagePath))
        {
            Directory.CreateDirectory(_imagePath);
        }
    }

    public async Task<byte[]> GetImageAsync(string fileName)
    {
        var filePath = Path.Combine(_imagePath, fileName);
        if (File.Exists(filePath))
        {
            return await File.ReadAllBytesAsync(filePath);
        }
        return null;
    }

    public async Task<bool> SaveImageAsync(string nameFile, IFormFile file)
    {
        if (file == null || file.Length == 0) return false;
        try
        {
            string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Images");

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            string filePath = Path.Combine(uploadsFolderPath, nameFile);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteImageAsync(string fileName)
    {
        try
        {
            string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Images");

            var filePath = Path.Combine(_imagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
            return false;
        }
    }
}
