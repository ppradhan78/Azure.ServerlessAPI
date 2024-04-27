using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using ServerlessAPI.Data.Infrastructure;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.EventHub
{
    public class FeedProcessEventHubCore : IFeedProcessEventHubCore
    {
        private readonly IConfiguration _configuration;
        public FeedProcessEventHubCore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PushEvent(string inputMessage)
        {
            int numOfEvents = 3;
            var producerClient = CreateEventHubClient();
            EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

            for (int i = 1; i <= numOfEvents; i++)
            {
                if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {inputMessage + i}"))))
                {
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            try
            {
                await producerClient.SendAsync(eventBatch);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }
        public async Task<string> PopEvent()
        {
            StringBuilder outputMessage = new StringBuilder();

            var processor = CreateEventProcessorClientClient();

            // Register handlers for processing events and handling errors
            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            // Start the processing
            await processor.StartProcessingAsync();

            // Wait for 30 seconds for the events to be processed
            await Task.Delay(TimeSpan.FromSeconds(30));

            // Stop the processing
            await processor.StopProcessingAsync();

            Task ProcessEventHandler(ProcessEventArgs eventArgs)
            {
                outputMessage.Append(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
                return Task.CompletedTask;
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
            {
                // Write details about the error 
                outputMessage.Append($"\t Partition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
                outputMessage.Append(eventArgs.Exception.Message);
                return Task.CompletedTask;
            }
            return outputMessage.ToString();
        }


        private EventHubProducerClient CreateEventHubClient()
        {
            return new EventHubProducerClient(CommonConstants.AzureEventHubConnection, CommonConstants.AzureEventHubName);


        }
        private EventProcessorClient CreateEventProcessorClientClient()
        {
            BlobContainerClient storageClient = new BlobContainerClient(CommonConstants.AzureStorageConnection, CommonConstants.blobContainerName);

            return new EventProcessorClient(storageClient, EventHubConsumerClient.DefaultConsumerGroupName,
  CommonConstants.AzureEventHubConnection, CommonConstants.AzureEventHubName);
        }

    }
}
