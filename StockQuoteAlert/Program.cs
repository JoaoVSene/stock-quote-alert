using StockQuoteAlert;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Text.Json;
using System.Globalization;
using System.Xml.Linq;

namespace StockQuoteAlert
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            string assetName;
            decimal sellPrice, buyPrice;
            Settings settings;

            string settingsPath = Settings.createPath("settings.json");
            settings = (await JsonFileReader.ReadAsync<Settings>(settingsPath))!;

            try
            {
                assetName = args[0];
                sellPrice = Convert.ToDecimal(args[1], new CultureInfo("en-US"));
                buyPrice = Convert.ToDecimal(args[2], new CultureInfo("en-US"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing args values:\n" + e.Message);
                return;
            }

            QuoteAPI api = new (settings.Api!.Token!);
            MailManager email = new (settings.Smtp!.Server!, settings.Smtp!.Port!, settings.Smtp!.Username!, settings.Smtp.Password!);

            QuoteTracker tracker = new(api, email, settings, assetName, sellPrice, buyPrice);
            await tracker.TrackerAsync();
        }
    }
}
