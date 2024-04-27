using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;

namespace TimerTrigger
{
    public class FeedProcessTimer
    {
        string StorageConnection = "DefaultEndpointsProtocol=https;AccountName=pkppractics;AccountKey=2QGGc/pyppQs7bXY9a0Q9Kt0T+3O9IpvTRfTdJliyRIX1vpsjqOwzxxF9iaPS5b9bScVmDlzSzT9+ASt16NVfA==;EndpointSuffix=core.windows.net";
        string queueName = "input";
        [FunctionName("FeedProcess")]
        public async Task Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
           var output = SendMessage();
            log.LogInformation($"C# Timer trigger Push Storage Queue:{output} at  {DateTime.Now}");
        }

        private QueueClient CreateQueueClient()
        {
            return new QueueClient(StorageConnection, queueName);
        }
        public string SendMessage()
        {
            var queueClient = CreateQueueClient();
            if (queueClient.Exists())
            {
                var output = queueClient.SendMessage(DateTime.Now.ToString());
                return output?.Value.MessageId;
            }
            else return "";
        }

    }
}
//0 * ****Every minute
//0 */2 * * * *	 Every 2 minute
//*/1 * * * * *	 Every second
//0 0 * * * *	 Every hour
//0 0 0 * * *	 Every day
//0 30 11 * * *	 Every day at 11:30:00
//0 0 5 - 10 * **Every hour between 5 to 10
//0 0 0 * * SAT	 Every saturday
//0 0 0 * * 6	 Every saturday
//0 0 0 * * 1-5	 Every workday (Monday to Friday)
//0 0 0 * * SAT, SUN	
//Every saturday and sunday
//0 0 0 1 1 *	 Every year 1st january
//0 0 0 1-7 * SAT	 Every first saturday of the month at 00:00:00