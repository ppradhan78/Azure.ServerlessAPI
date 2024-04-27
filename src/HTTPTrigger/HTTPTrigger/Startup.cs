using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ServerlessAPI.Data.Core.API;
using ServerlessAPI.Data.Core.Blob;
using ServerlessAPI.Data.Core.CosmosDB;
using ServerlessAPI.Data.Core.EventHub;
using ServerlessAPI.Data.Core.StorageQueue;

[assembly: FunctionsStartup(typeof(HTTPTrigger.Startup))]
namespace HTTPTrigger
{
    public class Startup : FunctionsStartup
    {
        //public IConfiguration Configuration { get; }
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddScoped<IOrdersCore, OrdersCore>();
            builder.Services.AddScoped<IFeedProcessBlobStorageCore, FeedProcessBlobStorageCore>();
            builder.Services.AddScoped<IFeedProcessQueueStorageCore, FeedProcessQueueStorageCore>();
            builder.Services.AddScoped<IFeedProcessEventHubCore, FeedProcessEventHubCore>();
            builder.Services.AddScoped<IFeedProcessCosmosDBCore, FeedProcessCosmosDBCore>();



        }
    }
}
