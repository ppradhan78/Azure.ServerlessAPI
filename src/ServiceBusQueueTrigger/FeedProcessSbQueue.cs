using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceBusQueueTrigger
{
    public class FeedProcessSbQueue
    {
        [FunctionName("FeedProcess")]
        public void Run([ServiceBusTrigger("pkppractics-sb-queues", Connection = "sbConnection")]string myQueueItem, ILogger _logger)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            if (!string.IsNullOrEmpty(myQueueItem))
            {
                _logger.LogInformation(myQueueItem);
                var output = JsonConvert.DeserializeObject<OrderSampleModel>(myQueueItem);
                if (output != null)
                {
                    _logger.LogInformation(output.Id);
                    _logger.LogInformation(output.OrderID.ToString());
                    _logger.LogInformation(output.EmployeeID.ToString());
                    _logger.LogInformation(output.CustomerID.ToString());
                    _logger.LogInformation(output.ShipName.ToString());
                }
            }
        }
    }
    public class OrderSampleModel
    {
        public string Id { get; set; }
        public string OrderID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustomerID { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}
