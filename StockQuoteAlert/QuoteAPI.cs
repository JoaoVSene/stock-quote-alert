using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Text.Json.Nodes;
using System.Globalization;

namespace StockQuoteAlert
{
    public class Stock
    {
        public decimal price;
        public string? currency;
        public Stock(decimal price,string currency)
        {
            this.price = price;
            this.currency = currency;
        }
    }
    public class QuoteAPI
    {
        private readonly string token;
        private readonly HttpClient client;

        public QuoteAPI(string token)
        {
            this.token = token;
            this.client = new HttpClient();
        }

        public async Task<Stock> GetPrice(string assetName)
        {
            string url = "https://brapi.dev/api/quote/" + assetName + "?token=" + this.token;
            string responseBody, currency = "";
            decimal regularMarketPrice = 0.0M;
            Stock stock;

            HttpResponseMessage response = await client.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Requesting API:\n" + ex.Message);
            }

            responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                var jsonObject = JsonNode.Parse(responseBody)!.AsObject();
                string node = (jsonObject["results"]![0]!["regularMarketPrice"]!).ToString();
                
                regularMarketPrice = Convert.ToDecimal(node, new CultureInfo("en-US"));
                currency = (jsonObject["results"]![0]!["currency"]!).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Parsing JSON:\n" + ex.Message);
            }

            stock = new Stock(regularMarketPrice, currency);

            return stock;
        }
    }
}
