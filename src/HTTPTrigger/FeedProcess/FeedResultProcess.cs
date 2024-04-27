using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessAPI.Data.Core.API;
using ServerlessAPI.Data.SimpleModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FeedProcess
{
    public class FeedResultProcess
    {
        #region Global Variable(s)
        private readonly IOrdersCore ordersCore;
        ILogger<FeedResultProcess> logger;
        private readonly IConfiguration configuration;
        #endregion

        public FeedResultProcess(IOrdersCore _ordersCore, ILogger<FeedResultProcess> _logger,  IConfiguration _configuration)
        {
            ordersCore = _ordersCore;
            this.logger = _logger;
            configuration = _configuration;
        }
        [FunctionName("FeedResultProcess")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string OrderInput = string.Empty;
            log.LogInformation("C# HTTP trigger function processed a request.");
            var output = new OrderOutputModel();
            try
            {
                //{ "OrderID":10248,"EmployeeID":5,"OrderDate":"2024-04-07T17:09:56.2552738+05:30","CustomerID":"VINET","RequiredDate":"2024-04-07T17:09:56.2553883+05:30","ShippedDate":"2024-04-07T17:09:56.2558456+05:30","MyProperty":null,"ShipVia":3,"Freight":32,"ShipName":"Vins et alcools Chevalier","ShipAddress":"59 rue de l'Abbaye","ShipCity":"Reims","ShipRegion":"RJ","ShipPostalCode":"51100","ShipCountry":"France"}
                var content = await new StreamReader(req?.Body).ReadToEndAsync();
                if (content != null)
                {
                    OrderInputModel input = JsonConvert.DeserializeObject<OrderInputModel>(content);
                    OrderInput = $"Order Id: {input.OrderID}";
                    log.LogInformation(OrderInput);
                    if (input != null)
                    {
                        output = await ordersCore.GetAllOrdersDetails(input.OrderID);
                        log.LogInformation($"output: {JsonConvert.SerializeObject(output)}");
                    }
                }
                return new OkObjectResult(output);

            }
            catch (Exception ex)
            {
                log.LogError("FeedResultProcess Exception. StackTrace,InnerException,Message@" + ex.StackTrace + "," + ex.InnerException + ex.Message + " , " + OrderInput);
                return new BadRequestResult();
            }
        }
    }
   
}
