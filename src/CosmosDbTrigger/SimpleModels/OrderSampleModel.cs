using Newtonsoft.Json;
using System;

namespace CosmosDbTrigger.SimpleModels
{
    public class OrderSampleModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string OrderID { get; set; }
        public EmployeeOrderSampleModel Employee { get; set; }
        public DateTime? OrderDate { get; set; }
        public CustomerOrderSampleModel Customer { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}
