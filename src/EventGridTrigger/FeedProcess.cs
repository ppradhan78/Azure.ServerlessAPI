// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Azure.Messaging.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Net;

namespace EventGridTrigger
{
    public  class FeedProcessFunction
    {
        public FeedProcessFunction()
        {
        }
        [FunctionName("FeedProcess")]
        [return: EventGrid(TopicEndpointUri = "EventGridTopicEndpoint", TopicKeySetting = "EventGridTopicKey")]
        public  void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());

        }
    }
    
}
