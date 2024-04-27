using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessAPI.Data.Core.Blob;
using ServerlessAPI.Data.Core.CosmosDB;
using ServerlessAPI.Data.Core.EventGrid;
using ServerlessAPI.Data.Core.EventHub;
using ServerlessAPI.Data.Core.StorageQueue;
using ServerlessAPI.Data.SimpleModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace ServerlessAPI.Data.Core.API
{
    public class OrdersCore : IOrdersCore
    {
        #region Global Variable(s)
        private readonly ILogger<OrdersCore> _logger;
        private readonly IConfiguration _configuration;
        static HttpClient client = new HttpClient();
        private string url = "https://demodata.grapecity.com/northwind/api/v1/Orders/";
        private readonly IFeedProcessQueueStorageCore _feedsQueueStorageCore;
        private readonly IFeedProcessEventHubCore _feedsEventHubCore;
        private readonly IFeedProcessCosmosDBCore _feedProcessCosmosDBCore;
        private readonly IFeedProcessBlobStorageCore _feedProcessBlobStorageCore;
        private readonly IFeedProcessEventGridCore _feedProcessEventGridCore;
        #endregion

        public OrdersCore(IConfiguration configuration,
            ILogger<OrdersCore> logger,
            IFeedProcessQueueStorageCore feedsQueueStorageCore,
            IFeedProcessEventHubCore feedsEventHubCore,
            IFeedProcessCosmosDBCore feedProcessCosmosDBCore,
            IFeedProcessBlobStorageCore feedProcessBlobStorageCore,
            IFeedProcessEventGridCore feedProcessEventGridCore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
            _feedsQueueStorageCore = feedsQueueStorageCore;
            _feedsEventHubCore = feedsEventHubCore;
            _feedProcessCosmosDBCore = feedProcessCosmosDBCore;
            _feedProcessBlobStorageCore = feedProcessBlobStorageCore;
            _feedProcessEventGridCore = feedProcessEventGridCore;
        }

        public async Task<OrderOutputModel> GetAllOrdersDetails(int OrdersId)
        {
            var output = new OrderOutputModel();

            //    var xx=_configuration["NorthWindConfig:BaseAddress"];
            //    string setting = _configuration.GetValue<string>("NorthWindConfig:BaseAddress");
            _logger.LogInformation($"GetAllOrdersDetails: OrdersId : {OrdersId}");

            output.CustomerOrder = await GetCustomerOrder(OrdersId);
            output.ShipperOrder = await GetShipperOrder(OrdersId);
            output.ProductsOrders = await GetProductsOrders(OrdersId);
            output.EmployeeOrder = await GetEmployeeOrder(OrdersId);
            output.OrderDetails = await GetOrderDetails(OrdersId);
            output.OrderList = await GetOrderList(OrdersId);
            output.Order = await GetOrder(OrdersId);
            return output;
        }

        #region Private Method(s)
        private async Task<List<OrderDetailsSampleModel>> GetOrderDetails(int ordersId)
        {
            _logger.LogInformation($"GetOrderDetails");
            try
            {
                List<OrderDetailsSampleModel> orderDetails = new List<OrderDetailsSampleModel>();
                HttpResponseMessage response = await client.GetAsync(url + ordersId + "/OrderDetails");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    orderDetails = JsonConvert.DeserializeObject<List<OrderDetailsSampleModel>>(result);
                }
                return orderDetails;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<EmployeeOrderSampleModel> GetEmployeeOrder(int ordersId)
        {
            _logger.LogInformation($"GetEmployeeOrder");

            EmployeeOrderSampleModel employeeorders = new EmployeeOrderSampleModel();
            HttpResponseMessage response = await client.GetAsync(url + ordersId + "/Employee");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                employeeorders = JsonConvert.DeserializeObject<EmployeeOrderSampleModel>(result);
                //SaveImage(employeeorders.employeeId, employeeorders.photo, result);
                await UploadtoBlobStroge(result, employeeorders.employeeId);
            }
            return employeeorders;

        }


        private async Task UploadtoBlobStroge(string input, string Id)
        {
            _logger.LogInformation($"UploadtoBlobStroge");
            try
            {
                var content = Encoding.UTF8.GetBytes(input);
                using (var fileStream = new MemoryStream(content))
                {
                    string FileName = Id + DateTime.Now.Ticks + ".txt";
                    var output = await _feedProcessBlobStorageCore.UploadFileBlobAsync(fileStream, FileName);
                    if (output != null)
                    {
                        _logger.LogInformation($"File to Uploaded  to Blob@:{output.AbsoluteUri}");
                    }
                }

            }
            catch (Exception)
            {

                _logger.LogError($"Fail to Upload file to Blob");
            }
        }

        private async Task<List<ProductsOrderSampleModel>> GetProductsOrders(int ordersId)
        {
            _logger.LogInformation($"GetProductsOrders");
            List<ProductsOrderSampleModel> orders = new List<ProductsOrderSampleModel>();
            HttpResponseMessage response = await client.GetAsync(url + ordersId + "/Products");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<ProductsOrderSampleModel>>(result);
            }
            return orders;

        }

        private async Task<ShipperOrderSampleModel> GetShipperOrder(int ordersId)
        {
            _logger.LogInformation($"GetShipperOrder");

            ShipperOrderSampleModel Shipperorders = new ShipperOrderSampleModel();
            HttpResponseMessage response = await client.GetAsync(url + ordersId + "/Shipper");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Shipperorders = JsonConvert.DeserializeObject<ShipperOrderSampleModel>(result);
            }
            return Shipperorders;

        }

        private async Task<List<OrderSampleModel>> GetOrderList(int ordersId)
        {
            _logger.LogInformation($"GetOrderList");
            try
            {
                List<OrderSampleModel> orders = new List<OrderSampleModel>();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    orders = JsonConvert.DeserializeObject<List<OrderSampleModel>>(result);
                }
                return orders;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<OrderSampleModel> GetOrder(int ordersId)
        {
            _logger.LogInformation($"GetOrder");

            OrderSampleModel order = new OrderSampleModel();
            HttpResponseMessage response = await client.GetAsync(url + ordersId);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<OrderSampleModel>(result);
                try
                {
                    _logger.LogInformation($"QueueStorage");

                    var output = await _feedsQueueStorageCore.SendMessage(result);
                    _logger.LogInformation($"Send Message to Queue responce : {output}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Fail to Send Message to Queue");
                }
                try
                {
                    _logger.LogInformation($"EventHub");
                    result = "{EventHub:}" + result;
                    await _feedsEventHubCore.PushEvent(result);
                    _logger.LogError($"Send Event to EventHub");
                    var output = await _feedsEventHubCore.PopEvent();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Fail to Event Message to EventHub");
                }
                try
                {
                    _logger.LogInformation($"EventGrid");

                    await _feedProcessEventGridCore.PushEvent(result);
                    _logger.LogError($"Send Event to EventGrid");
                    //var output = await _feedProcessEventGridCore.PopEvent();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Fail to Send Event to EventGrid");
                }
                try
                {
                    _logger.LogInformation($"CosmosDB");
                    await _feedProcessCosmosDBCore.AddRecord(order);
                    _logger.LogError($"Record Added to CosmosDB");
                }
                catch (Exception)
                {
                    _logger.LogError($"Fail to Add Record to CosmosDB");
                }
            }
            return order;
        }

        private async Task<CustomerOrderSampleModel> GetCustomerOrder(int ordersId)
        {
            _logger.LogInformation($"GetCustomerOrder");

            CustomerOrderSampleModel Custorder = new CustomerOrderSampleModel();
            HttpResponseMessage response = await client.GetAsync(url + ordersId + "/Customer");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Custorder = JsonConvert.DeserializeObject<CustomerOrderSampleModel>(result);
            }
            return Custorder;
        }

        private void SaveImage(string employeeId, byte[] bytes, string Jsontext)
        {
            _logger.LogInformation($"SaveImage");

            try
            {
                string path = Directory.GetCurrentDirectory() + "\\";
                bool exists = System.IO.Directory.Exists(path);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                _logger.LogInformation($"path@{path}");

                File.WriteAllText(path + employeeId + "_" + DateTime.Now.Ticks + ".txt", Jsontext);
            }
            catch (Exception)
            {
            }
        }

        //private async Task UploadtoBlobStroge()
        //{
        //    _logger.LogInformation($"UploadtoBlobStroge");
        //    try
        //    {
        //        string DirectoryPath = Directory.GetCurrentDirectory()+"\\";// Path.Combine(, "UploadFiles");
        //        if (Directory.Exists(DirectoryPath))
        //        {
        //            var fileEntries = Directory.GetFiles(DirectoryPath, "*.*", SearchOption.AllDirectories).Where(file => new string[] { ".txt" }.Contains(Path.GetExtension(file)));
        //            foreach (string fileName in fileEntries)
        //            {
        //                string FileName = Path.GetFileName(fileName);
        //                var fileStream = System.IO.File.OpenRead(fileName);
        //                if (fileStream != null)
        //                {
        //                    var output = await _feedProcessBlobStorageCore.UploadFileBlobAsync(fileStream, FileName);
        //                    if (output != null)
        //                    {
        //                        _logger.LogInformation($"File to Uploaded  to Blob@:{output.AbsoluteUri}");
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        _logger.LogError($"Fail to Upload file to Blob");
        //    }
        //}

        #endregion
    }
}
