using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using ServerlessAPI.Data.SimpleModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Container = Microsoft.Azure.Cosmos.Container;

namespace ServerlessAPI.Data.Core.CosmosDB
{
    public class FeedProcessCosmosDBCore : IFeedProcessCosmosDBCore
    {
        private readonly string CosmosDBAccountUri = "https://feedprocessserverless-dbs.documents.azure.com:443/";
        private readonly string CosmosDBAccountPrimaryKey = "YbXfRRaXg9aDlu8d0xvzlXQucMnx7jwNLz4syJFHt7TfFaN4hS69mfD4Lr2EJ5vQWRHiXDyQ18kyACDbBDbOiA==";
        private readonly string CosmosDbName = "Northwind";
        private readonly string CosmosDbContainerName = "Order";
        private readonly IConfiguration _configuration;
        public FeedProcessCosmosDBCore(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task AddRecord(OrderSampleModel order)
        {
           
            try
            {
                var container = ContainerClient();
                var response = await container.CreateItemAsync(order, new PartitionKey(order.CustomerID));
               
            }
            catch (Exception ex)
            {
               
            }
        }

        private Container ContainerClient()
        {
            CosmosClient cosmosDbClient = new CosmosClient(CosmosDBAccountUri, CosmosDBAccountPrimaryKey);
            Container containerClient = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return containerClient;
        }
    }
}
