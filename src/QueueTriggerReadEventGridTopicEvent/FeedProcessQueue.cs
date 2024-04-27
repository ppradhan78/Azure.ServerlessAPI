using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QueueTriggerReadEventGridTopicEvent
{
    public class FeedProcessQueue
    {
        [FunctionName("FeedProcess")]
        public void Run([QueueTrigger("input", Connection = "StorageConnection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function Recive Data from Event Grid Topic: {myQueueItem}");
            if (!string.IsNullOrEmpty(myQueueItem))
            {
                var output = JsonConvert.DeserializeObject<EventRecived>(myQueueItem);
                if (output != null && output.Data != null)
                {
                    log.LogInformation(output.Data.orderId.ToString());
                    log.LogInformation(output.Data.productId.ToString());
                    log.LogInformation(output.Data.quantity);
                    log.LogInformation(output.Data.discount != null ? output.Data.discount.ToString() : "NA");
                    log.LogInformation(output.Data.unitPrice.ToString());
                    log.LogInformation("\n");

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
    public class EventRecived
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public OrderDetailsSampleModel Data { get; set; }
        public string EventType { get; set; }
        public string dataVersion { get; set; }
        public string metadataVersion { get; set; }
        public DateTime EventTime { get; set; }
        public string Topic { get; set; }
    }
}
