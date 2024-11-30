using MongoDbProduct.Entities;

namespace MongoDbProduct.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public int Piece { get; set; }
        public string ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
    }
}
