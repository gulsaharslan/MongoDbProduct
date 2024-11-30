using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDbProduct.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }
        public int Piece { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
