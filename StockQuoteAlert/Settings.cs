using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.IO;

namespace StockQuoteAlert
{
    public class APISettings
    {
        public string? Token { get; set; }
        public int Delay { get; set; }
    }
    public class SmtpSettings
    {
        public string? Server { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
    public class Settings
    {
        public APISettings? Api { get; set; }
        public SmtpSettings? Smtp { get; set; }
        public string? Sender { get; set; }
        public List<string>? Recipients { get; set; }

        public static string createPath(string localPath)
        {
            return Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName, localPath);
        }
    }
    public static class JsonFileReader
    {
        public static async Task<T> ReadAsync<T>(string filePath)
        {
            var stream = File.OpenRead(filePath);
            return (await JsonSerializer.DeserializeAsync<T>(stream))!;
        }
    }
}
