using JobManagement.Application.Contracts.Services;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace JobManagement.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        public FileStorageService(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._configuration = configuration;
        }


        //Upload Job Application pdf file to Supabase
        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, string bucket)
        {
            //get supabase configurations from appsettings.json
            var supabaseUrl = _configuration["Supabase:Url"];
            var apiKey = _configuration["Supabase:ApiKey"];

            //Change the filename to a unique one
            var extension = Path.GetExtension(fileName);
            var originalName = Path.GetFileNameWithoutExtension(fileName);
            var safeName = string.Join("_", originalName.Split(Path.GetInvalidFileNameChars()));
            var uniqueFileName = $"{Guid.NewGuid()}-{safeName}{extension}";

            //Set the ApplicationDocumentKey and url
            var key = uniqueFileName;
            var url = $"{supabaseUrl}/storage/v1/object/{bucket}/{key}";

            //Send an http POST request to Supabase 
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("apikey", apiKey);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            request.Content = content;

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return key;

        }
    }
}
