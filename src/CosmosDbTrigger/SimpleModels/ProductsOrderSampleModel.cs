namespace CosmosDbTrigger.SimpleModels
{
    public class ProductsOrderSampleModel
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int supplierId { get; set; }
        public int categoryId { get; set; }
        public string quantityPerUnit { get; set; }
        public decimal unitPrice { get; set; }
        public int unitsInStock { get; set; }
        public int unitsOnOrder { get; set; }
        public int reorderLevel { get; set; }
        public bool discontinued { get; set; }
    }
}
