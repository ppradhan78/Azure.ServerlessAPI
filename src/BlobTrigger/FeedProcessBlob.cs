using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlobTrigger
{
    public class FeedProcessBlob
    {
        [FunctionName("FeedProcess")]
        public void Run([BlobTrigger("input/{name}", Connection = "StorageConnection")] Stream myBlob, string name, ILogger log)
        {
            var output = new List<OrderDetailsSampleModel>();
            if (myBlob != null)
            {
                using (var reader = new StreamReader(myBlob))
                {
                    var filecontent = reader.ReadToEnd();
                     output = JsonConvert.DeserializeObject<List<OrderDetailsSampleModel>>(filecontent);
                    if (output.Any())
                    {
                        foreach (var item in output)
                        {
                            log.LogInformation(item.orderId.ToString());
                            log.LogInformation(item.productId.ToString());
                            log.LogInformation(item.quantity);
                            log.LogInformation(item.discount != null ? item.discount.ToString() : "NA");
                            log.LogInformation(item.unitPrice.ToString());
                            log.LogInformation("\n");

                        }
                    }
                    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
                    log.LogInformation($"C# Blob trigger function read file : {filecontent} ");

                }
            }
        }
    }
    public class OrderDetailsSampleModel
    {
        public int orderId { get; set; }
        public int productId { get; set; }
        public decimal unitPrice { get; set; }
        public string quantity { get; set; }
        public Single? discount { get; set; }
    }
}
