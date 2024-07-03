using Microsoft.AspNetCore.Http;

namespace Server.Application.Interfaces;

public interface IFileSerice
{
    Task<byte[]> GetImageAsync(string fileName);
    Task<bool> SaveImageAsync(string nameFile, IFormFile file);
    Task<bool> DeleteImageAsync(string fileName);
}
