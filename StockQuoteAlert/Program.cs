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

            Stock actualPrice;
            string action;

            while (true)
            {
                actualPrice = await api.GetPrice(assetName);
                Console.WriteLine("Regular Market Price: " + actualPrice.price + " " + actualPrice.currency + ".");
                Console.WriteLine(sellPrice + " " + buyPrice);

                if (actualPrice.price > sellPrice)
                {
                    Console.WriteLine("Sending emails to sell shares.");
                    email.SendEmailMessage(settings.Sender!, settings.Recipients!, actualPrice, assetName, "sell");
                }
                else if (actualPrice.price < buyPrice)
                {
                    Console.WriteLine("Sending emails to buy shares.");
                    email.SendEmailMessage(settings.Sender!, settings.Recipients!, actualPrice, assetName, "buy");
                }

                await Task.Delay(settings.Api!.Delay!);
            }
        }
    }
}
