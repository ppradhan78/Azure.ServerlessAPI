namespace ServerlessAPI.Data.Infrastructure
{
    public static class CommonConstants
    {

        public static string AzureStorageConnection = "DefaultEndpointsProtocol=https;AccountName=azpracticsstgact123;AccountKey=GfCqNBP5PZvb4hlTZkR7vU6wAHjmMVQxD7RN8E/WZqAAy5y2dx2oRoHSA4pw46vWV41TNVUC9/+7+AStWbimXA==;EndpointSuffix=core.windows.net";
        public static string blobContainerName = "input";
        public static string AzureStorageQueueName = "input";
        public static string AzureEventHubConnection = "Endpoint=sb://feedprocessserverlessapi-eventhub-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=I1eJQSxgtAsN0iN479dQN1gzYUVlPc6H8+AEhKFjIPI=";
        public static string AzureEventHubNameSpace = "feedprocessserverlessapi-eventhub-ns.servicebus.windows.net";
        public static string AzureEventHubName = "feedprocessserverlessapi-eventhub";
        public static string AzureEventGridTopicEndpoint = "https://feedprocessserverlessapi-topic.westus-1.eventgrid.azure.net/api/events";
        public static string AzureEventGridTopicKey = "AQO6iaSQtBI0zU3ILgzn6guc/TfE2hOPOJpCKMmle3A=";
        public static string AzureEventGridTopicName = "feedprocessserverlessapi-topic";
        public static string AzureEventGridSubscription = "feedprocessserverlessapi-topic-sub";
        private static string CosmosDBAccountUri = "https://feedprocessserverless-dbs.documents.azure.com:443/";
        private static string CosmosDBAccountPrimaryKey = "YbXfRRaXg9aDlu8d0xvzlXQucMnx7jwNLz4syJFHt7TfFaN4hS69mfD4Lr2EJ5vQWRHiXDyQ18kyACDbBDbOiA==";
        private static string CosmosDbName = "Northwind";
        private static string CosmosDbContainerName = "Order";



    }
}
