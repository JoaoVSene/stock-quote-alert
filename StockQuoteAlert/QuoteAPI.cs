using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Text.Json.Nodes;
using System.Globalization;

public class QuoteAPI
{
	private readonly string token;
    private readonly HttpClient client;

    public QuoteAPI(string token)
    {
		this.token = token;
        this.client = new HttpClient();
    }

	public async Task<decimal> GetPrice(string assetName){

		string url = "https://brapi.dev/api/quote/" + assetName + "?token=" + this.token;
        string responseBody;
        decimal regularMarketPrice = 0.0M;

        HttpResponseMessage response = await client.GetAsync(url);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch(Exception ex) { 
            Console.WriteLine("Error Requesting API:\n"+ex.Message);
        }

        responseBody = await response.Content.ReadAsStringAsync();

        try
        {
            var jsonObject = JsonNode.Parse(responseBody)!.AsObject();
            var node = jsonObject["results"]![0]!["regularMarketPrice"]!;
            regularMarketPrice = Convert.ToDecimal(node.ToString(), new CultureInfo("en-US"));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Parsing JSON:\n"+ex.Message);
        }
        
        return regularMarketPrice;
    }
}
