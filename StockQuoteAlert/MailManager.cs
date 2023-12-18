using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace StockQuoteAlert
{
    internal class MailManager
    {
        public string server;
        public int port;
        public string username;
        public string password;

        public MailManager(string server, int port, string username, string password)
        {
            this.server = server;
            this.port = port;
            this.username = username;
            this.password = password;
        }

        public static bool EmailAddressValidation(string emailAddress)
        {
            try
            {
                Regex regexExpression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                if (regexExpression.IsMatch(emailAddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string CreateBody(decimal actualPrice, string symbol, string assetName, string action)
        {
            string body = "Hello,\n\n" +
                          "I hope this message finds you well. I wanted to bring to your attention some recent " +
                          "developments regarding " + assetName + ". According to our analysis, we advise you " +
                          "to " + action + " the shares. The actual price is " + actualPrice + " " + symbol + "." +
                          "\n\nThank you for your attention.";
            return body;
        }

        public void SendEmailMessage(string sender, List<string> recipients, Stock stock, string assetName,string action)
        {
            SmtpClient client;

            try
            {
                client = new SmtpClient(this.server, this.port)
                {
                    Credentials = new NetworkCredential(this.username, this.password),
                    EnableSsl = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid SMTP Credential: " + ex);
                return;
            }

            string subject = "Stock Quote Alert - " + assetName;

            foreach (string recipient in recipients)
            {
                if (!EmailAddressValidation(recipient))
                {
                    Console.WriteLine("Invalid email recipient: " + recipient);
                    return;
                }

                string body = CreateBody(stock.price,stock.currency!, assetName,action);
                
                try
                {
                    MailMessage mailMessage = new (sender, recipient, subject, body);
                    client.Send(mailMessage);

                    Console.WriteLine("Email sent to " + recipient + " at " + DateTime.Now.ToString() + ".");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex);
                    return;
                }
            }
            client.Dispose();
        }
    }
}
