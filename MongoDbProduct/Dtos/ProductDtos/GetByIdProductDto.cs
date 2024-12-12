using System.ComponentModel.DataAnnotations.Schema;

namespace MongoDbProduct.Dtos.ProductDtos
{
    public class GetByIdProductDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public string? SavedUrl { get; set; }
        public string? SavedFileName { get; set; }
    }
}
