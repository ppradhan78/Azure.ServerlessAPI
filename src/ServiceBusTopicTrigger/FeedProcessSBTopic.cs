using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceBusTopicTrigger
{
    public class FeedProcessSBTopic
    {
        //private readonly ILogger<FeedProcessSBTopic> _logger;

        //public FeedProcessSBTopic(ILogger<FeedProcessSBTopic> log)
        //{
        //    _logger = log;
        //}

        [FunctionName("FeedProcess")]
        public void Run([ServiceBusTrigger(topicName:"pkppractics-sb-topic",subscriptionName: "pkppractics-sb-topic-sub", Connection = "sbConnection")]string mySbMsg, ILogger _logger)
        {
            _logger.LogInformation( "Start");
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            _logger.LogInformation("End");
            if (!string.IsNullOrEmpty(mySbMsg))
            {
                _logger.LogInformation(mySbMsg);
                var output = JsonConvert.DeserializeObject<OrderSampleModel>(mySbMsg);
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
