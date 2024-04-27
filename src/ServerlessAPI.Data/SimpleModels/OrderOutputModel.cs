using System.Collections.Generic;

namespace ServerlessAPI.Data.SimpleModels
{
    public  class OrderOutputModel
    {
        public List<OrderSampleModel> OrderList { get; set; }
        public OrderSampleModel Order { get; set; }
        public CustomerOrderSampleModel CustomerOrder { get; set; }
        public EmployeeOrderSampleModel EmployeeOrder { get; set; }
        public List<OrderDetailsSampleModel> OrderDetails{ get; set; }
        public ShipperOrderSampleModel ShipperOrder { get; set; }
        public List<ProductsOrderSampleModel> ProductsOrders { get; set; }
    }
}
