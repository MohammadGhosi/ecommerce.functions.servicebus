using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using ecommerce.functions.servicebus.Dtos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ecommerce.functions.servicebus
{
    public class Function
    {
        [FunctionName("S1Subscriber")]
        public async Task Run([ServiceBusTrigger("mytopic", "S1", Connection = "mytopicConnectionStr")]string mySbMsg, ILogger logger)
        {
            try
            {
                var userObj = JsonConvert.DeserializeObject<User>(mySbMsg);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7078");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using HttpResponseMessage res = await client.PostAsJsonAsync<User>("api/Users", userObj);
                using HttpContent content = res.Content;
                string data = await content.ReadAsStringAsync();
            }
            catch (Exception ex)
            { 
            
            }
            
            logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
