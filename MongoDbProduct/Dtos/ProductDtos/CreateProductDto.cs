using MongoDbProduct.Entities;

namespace MongoDbProduct.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public IFormFile Photo { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public string? SavedUrl { get; set; }
        public string? SavedFileName { get; set; }
    }

}
