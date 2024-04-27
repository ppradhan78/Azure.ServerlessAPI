using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventHubTrigger
{
    public  class FeedProcessEventHub
    {
        public FeedProcessEventHub()
        {
                
        }
        [FunctionName("FeedProcess")]
        public  async Task Run([EventHubTrigger("pkppractics-eh", Connection = "eventHubConnection")] EventData[] events, ILogger log)
        {
            foreach (EventData eventData in events)
            {
                try
                {
                    if (eventData?.EventBody is null)
                        continue;

                    string messageBody = Encoding.UTF8.GetString(eventData.EventBody);

                    if (string.IsNullOrEmpty(messageBody))
                    {
                        log.LogError("Error: Invalid Request");
                        continue;
                    }
                    else
                    {
                        log.LogInformation(messageBody);
                        var output = JsonConvert.DeserializeObject<OrderDetailsSampleModel>(messageBody);
                        if (output != null)
                        {
                            log.LogInformation(output.quantity);
                            log.LogInformation(output.orderId.ToString());
                            log.LogInformation(output.productId.ToString());
                            log.LogInformation(output.unitPrice.ToString());
                            log.LogInformation(output.discount.ToString());
                        }
                    }
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    log.LogError($"Error: \"{e.InnerException.Message}\" was caught while processing the request");
                }
            }
        }
    }

    public class OrderDetailsSampleModel
    {
        public int orderId { get; set; }
        public int productId { get; set; }
        public decimal unitPrice { get; set; }
        public string quantity { get; set; }
        public Single? discount { get; set; }
    }
}
