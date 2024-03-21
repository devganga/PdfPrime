using System.Text.Json;

namespace PdfPrime.AppSettings
{
    public record ApplicationOptions
    {
        public string? ApplicationName { get; set; }
        public string? ApplicationVersion { get; set; }
        public string? ApplicationDescription { get; set; }
        public string? ApplicationOutputPath { get; set; }
        public EndpointOptions? Endpoint { get; set; }
        public EncryptOptions? Encrypt { get; set; }

        public string GetTempFileName()
        {
            return Path.GetTempFileName();
        }
        public string GetApplicationTempPath()
        {
            return Path.Combine(Path.GetTempPath(), ApplicationName ?? throw new Exception($"Serialize object {nameof(ApplicationName)} shouldn't be null"));
        }
        public string GetApplicationDataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName ?? throw new Exception($"Serialize object {nameof(ApplicationName)} shouldn't be null"));
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        }

    }
}
