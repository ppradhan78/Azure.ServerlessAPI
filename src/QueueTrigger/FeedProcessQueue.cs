using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QueueTrigger
{
    public class FeedProcessQueue
    {
        [FunctionName("FeedProcess")]
        public void Run([QueueTrigger("input", Connection = "StorageConnection")]string myQueueItem, ILogger log)
        {
            var output = new List<OrderDetailsSampleModel>();
            output = JsonConvert.DeserializeObject<List<OrderDetailsSampleModel>>(myQueueItem);
            if (output.Any())
            {
                foreach (var item in output)
                {
                    log.LogInformation(item.orderId.ToString());
                    log.LogInformation(item.productId.ToString());
                    log.LogInformation(item.quantity);
                    log.LogInformation(item.discount != null ? item.discount.ToString() : "NA");
                    log.LogInformation(item.unitPrice.ToString());
                    log.LogInformation("\n");

                }
            }
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
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
