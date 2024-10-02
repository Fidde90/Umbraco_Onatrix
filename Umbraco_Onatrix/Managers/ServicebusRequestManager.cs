using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Diagnostics;
using Umbraco_Onatrix.Models;

namespace Umbraco_Onatrix.Managers
{
    public class ServicebusRequestManager
    {
        private readonly IConfiguration _configuration;

        public ServicebusRequestManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(EmailRequestModel email, string queue)
        {
            string cs = GetConnectionString("ServiceBusEmailCS");

            if (email != null && !string.IsNullOrEmpty(cs) && !string.IsNullOrEmpty(queue))
            {
                await using var client = new ServiceBusClient(cs);
                ServiceBusSender sender = client.CreateSender(queue);
                var json = JsonConvert.SerializeObject(email);
                ServiceBusMessage message = new(json) { ContentType = "application/json" };
                try
                {                
                    await sender.SendMessageAsync(message);
                }
                catch (Exception ex) { Debug.WriteLine($"ERROR :: Sending to email queue {ex.Message}"); return false; }
                return true;
            }
            return false;
        }

        public string GetConnectionString(string service)
        {
            return _configuration[$"ConnectionStrings:{service}"] ?? null!;
        }
    }
}
