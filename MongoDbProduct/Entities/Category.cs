using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDbProduct.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
