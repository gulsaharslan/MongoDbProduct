namespace MongoDbProduct.Dtos.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
    }
}
