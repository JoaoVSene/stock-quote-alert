using StockQuoteAlert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockQuoteAlert
{
    internal class QuoteTracker
    {
        private readonly QuoteAPI api;
        private readonly MailManager email;
        private readonly Settings settings;
        private readonly string assetName;
        private readonly decimal sellPrice;
        private readonly decimal buyPrice;

        public QuoteTracker(QuoteAPI api, MailManager email, Settings settings, string assetName, decimal sellPrice, decimal buyPrice)
        {
            this.api = api;
            this.email = email;
            this.settings = settings;
            this.assetName = assetName;
            this.sellPrice = sellPrice;
            this.buyPrice = buyPrice;
        }

        public async Task TrackerAsync()
        {
            Stock actualPrice;

            while (true)
            {
                actualPrice = await api.GetPrice(this.assetName!);
                Console.WriteLine("Regular Market Price: " + actualPrice.price + " " + actualPrice.currency +
                    " at " + DateTime.Now.ToString() + ".");

                if (actualPrice.price >= sellPrice)
                {
                    Console.WriteLine("Sending emails to sell shares.");
                    email.SendEmailMessage(settings.Sender!, settings.Recipients!, actualPrice, assetName, "sell");
                }
                else if (actualPrice.price <= buyPrice)
                {
                    Console.WriteLine("Sending emails to buy shares.");
                    email.SendEmailMessage(settings.Sender!, settings.Recipients!, actualPrice, assetName, "buy");
                }
                else
                {
                    Console.WriteLine("Not sending any emails.");
                }

                await Task.Delay(settings.Api!.Delay!);
            }
        }
    }
}
