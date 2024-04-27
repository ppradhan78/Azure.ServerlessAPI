using CosmosDbTrigger.SimpleModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CosmosDbTrigger
{
    public  class FeedProcessCosmosDbTrigger
    {

        [FunctionName("FeedProcess")]
        public  void Run([CosmosDBTrigger(
            databaseName: "Northwind",
            collectionName: "OrderDetails",
            ConnectionStringSetting = "ConnectionStringSetting",
            CreateLeaseCollectionIfNotExists  = true,
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
                var inputValue = input[0].ToString();
                var output = JsonConvert.DeserializeObject<OrderDetailsSampleModel>(inputValue);
                if (output != null)
                {
                    log.LogInformation(output.quantity);
                    log.LogInformation(output.orderId.ToString());
                    log.LogInformation(output.productId.ToString());
                    log.LogInformation(output.unitPrice.ToString());
                    log.LogInformation(output.discount.ToString());
                }
            }
        }
    }
}
