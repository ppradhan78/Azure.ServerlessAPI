using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using ServerlessAPI.Data.Infrastructure;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.StorageQueue
{
    public class FeedProcessQueueStorageCore: IFeedProcessQueueStorageCore
    {
        private readonly IConfiguration _configuration;

        public FeedProcessQueueStorageCore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SendMessage(string inputMessage)
        {
            var queueClient = CreateQueueClient();
            queueClient.SendMessage(inputMessage);
            var output =  queueClient.SendMessage(inputMessage);
            return output?.Value.MessageId;
        }

        private QueueClient CreateQueueClient()
        {
            //return new QueueClient(_configuration.QueueConnectionString, _configuration.QueueName);
            return new QueueClient(CommonConstants.AzureStorageConnection, CommonConstants.AzureStorageQueueName);

        }
    }
}
