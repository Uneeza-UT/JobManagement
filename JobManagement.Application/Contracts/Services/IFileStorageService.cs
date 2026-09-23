namespace JobManagement.Application.Contracts.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, string bucket);
}
