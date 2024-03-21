using PdfPrime.AppSettings;
using System.Security.Cryptography;
using System.Text;

namespace PdfPrime.Services
{
    public interface IFileSecureService
    {
        void Encrypt(string fileName, string content);
        string Decrypt(string fileName);
    }
    public class FileSecureService : IFileSecureService
    {
        private readonly EncryptOptions options;

        public FileSecureService(IBaseService baseService)
        {
            options = baseService.Options.Encrypt ?? throw new Exception("Application Encrypt Options");
        }

        public void Encrypt(string fileName, string content)
        {
            using (FileStream fileStream = new(fileName, FileMode.OpenOrCreate))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key =
                    {
                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                    };

                    if (options.SecretKey != null)
                    {
                        key = Encoding.UTF8.GetBytes(options.SecretKey);
                    }
                    aes.Key = key;

                    byte[] iv = aes.IV;
                    fileStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new(
                        fileStream,
                        aes.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        // By default, the StreamWriter uses UTF-8 encoding.
                        // To change the text encoding, pass the desired encoding as the second parameter.
                        // For example, new StreamWriter(cryptoStream, Encoding.Unicode).
                        using (StreamWriter encryptWriter = new(cryptoStream))
                        {
                            encryptWriter.WriteLine(content);
                        }
                    }
                }
            }
        }
        public string Decrypt(string fileName)
        {
            using (FileStream fileStream = new(fileName, FileMode.Open))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] iv = new byte[aes.IV.Length];
                    int numBytesToRead = aes.IV.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    byte[] key =
                    {
                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                    };

                    if (options.SecretKey != null)
                    {
                        key = Encoding.UTF8.GetBytes(options.SecretKey);
                    }
                    using (CryptoStream cryptoStream = new(
                       fileStream,
                       aes.CreateDecryptor(key, iv),
                       CryptoStreamMode.Read))
                    {
                        // By default, the StreamReader uses UTF-8 encoding.
                        // To change the text encoding, pass the desired encoding as the second parameter.
                        // For example, new StreamReader(cryptoStream, Encoding.Unicode).
                        using (StreamReader decryptReader = new(cryptoStream))
                        {
                            string decryptedMessage = decryptReader.ReadToEndAsync().Result;
                            Console.WriteLine($"The decrypted original message: \n{decryptedMessage}");
                            return decryptedMessage;
                        }
                    }
                }
            }
        }
    }
}
