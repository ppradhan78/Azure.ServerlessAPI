using System;

namespace ServerlessAPI.Data.SimpleModels
{
    public class OrderDetailsSampleModel
    {
        public int orderId { get; set; }
        public int productId { get; set; }
        public decimal unitPrice { get; set; }
        public string quantity { get; set; }
        public Single? discount { get; set; }
    }
}
