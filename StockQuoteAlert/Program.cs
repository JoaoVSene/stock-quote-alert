using StockQuoteAlert;
using System.Text.Json;
using static QuoteAPI;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        string assetName;
        decimal sellPrice, buyPrice, value;
        Settings settings;

        string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "settings.json");

        settings = await JsonFileReader.ReadAsync<Settings>(path);

        try
        {
            assetName = args[0];
            sellPrice = decimal.Parse(args[1]);
            buyPrice = decimal.Parse(args[2]);
        }
        catch (Exception e){
            Console.WriteLine("Error parsing args values:\n"+e.Message);
            return;
        }

        Console.WriteLine(settings.Api);

        var api = new QuoteAPI(settings.Api.Token);

        value = await api.GetPrice(assetName);
        
        Console.WriteLine("Value: " + value);
    }
}
