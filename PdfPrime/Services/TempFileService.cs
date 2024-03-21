using Microsoft.Extensions.Logging;
using PdfPrime.AppSettings;
using PdfPrime.Services;
using System.Diagnostics;

namespace PdfPrime.Services
{
    public interface ITempFileService
    {
        Task<string> CreateTempFileAsync();
        Task<string> CreateAsync(string fileName, string fileExtension);
        Task<bool> UpdateAsync(string fileName, string content);
        Task<string> ReadAsync(string fileName);
        Task<bool> DeleteAsync(string fileName);
    }
    public class TempFileService : ITempFileService
    {
        private readonly ILogger<TempFileService> _logger;
        private readonly ApplicationOptions _applicationDataOptions;

        public TempFileService(ILogger<TempFileService> logger, IBaseService baseService)
        {
            _logger = logger;
            _applicationDataOptions = baseService.Options;
        }
        public async Task<string> CreateTempFileAsync()
        {
            var watch = Stopwatch.StartNew();
            // Get the full name of the newly created Temporary file. 
            // Note that the GetTempFileName() method actually creates
            // a 0-byte file and returns the name of the created file.
            var fileName = Path.GetTempFileName();
            _logger.LogInformation("File created {0}", fileName);
            // Craete a FileInfo object to set the file's attributes
            FileInfo fileInfo = new FileInfo(fileName);

            // Set the Attribute property of this file to Temporary. 
            // Although this is not completely necessary, the .NET Framework is able 
            // to optimize the use of Temporary files by keeping them cached in memory.
            fileInfo.Attributes = FileAttributes.Temporary;
            _logger.LogDebug("Set File Attributes Temporary {0}", fileName);
            watch.Stop();
            _logger.LogInformation("Created TempFile '{0}' in {1} ms", fileName, watch.ElapsedMilliseconds);
            return await Task.FromResult(fileName);
        }
        public async Task<string> CreateAsync(string fileName, string fileExtension)
        {
            var watch = Stopwatch.StartNew();
            // Get the full name of the newly created Temporary file. 
            // Note that the GetTempFileName() method actually creates
            // a 0-byte file and returns the name of the created file.
            var filePath = Path.Combine(_applicationDataOptions.GetApplicationTempPath(), $"{fileName}.{fileExtension}");

            if (File.Exists(filePath))
            {
                _logger.LogError("File {0}.{1} already existed", filePath, fileExtension);
            }


            // Craete a FileInfo object to set the file's attributes
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.Create();
            // Set the Attribute property of this file to Temporary. 
            // Although this is not completely necessary, the .NET Framework is able 
            // to optimize the use of Temporary files by keeping them cached in memory.
            fileInfo.Attributes = FileAttributes.Temporary;

            //StreamWriter streamWriter = File.AppendText(fileName);
            ////streamWriter.WriteLine(content);
            //streamWriter.Flush();
            //streamWriter.Close();
            //
            _logger.LogInformation("File created {0}", filePath);
            // Craete a FileInfo object to set the file's attributes
            //FileInfo fileInfo = new FileInfo(filePath);


            // Set the Attribute property of this file to Temporary. 
            // Although this is not completely necessary, the .NET Framework is able 
            // to optimize the use of Temporary files by keeping them cached in memory.
            //fileInfo.Attributes = FileAttributes.Temporary;
            _logger.LogDebug("Set File Attributes Temporary {0}.{1}", filePath, fileExtension);
            watch.Stop();
            _logger.LogInformation("Created File '{0}.{1}' in {2} ms", fileName, fileExtension, watch.ElapsedMilliseconds);
            return await Task.FromResult(filePath);
        }
        public async Task<bool> UpdateAsync(string fileName, string content)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation("updating File {0}", fileName);
            StreamWriter streamWriter = File.AppendText(fileName);
            streamWriter.WriteLine(content);
            streamWriter.Flush();
            streamWriter.Close();
            //_logger.LogInformation("updated File {0}", fileName);
            watch.Stop();
            _logger.LogInformation("updated File '{0}' in {1} ms", fileName, watch.ElapsedMilliseconds);
            return await Task.FromResult(true);
        }
        public async Task<string> ReadAsync(string fileName)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation("Opening File {0}", fileName);
            StreamReader streamReader = File.OpenText(fileName);
            _logger.LogInformation("Reading File {0}", fileName);
            watch.Stop();
            _logger.LogInformation("Readed File '{0}' in {1} ms", fileName, watch.ElapsedMilliseconds);
            return await streamReader.ReadToEndAsync();
        }
        public async Task<bool> DeleteAsync(string fileName)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation("deleting File {0}", fileName);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            else
            {
                _logger.LogError("File {0} not exists", fileName);
            }
            watch.Stop();
            _logger.LogInformation("Deleted File '{0}' in {1} ms", fileName, watch.ElapsedMilliseconds);
            return await Task.FromResult(true);
        }
    }
}
