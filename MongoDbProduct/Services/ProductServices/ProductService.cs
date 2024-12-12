using AutoMapper;
using MongoDB.Driver;
using MongoDbProduct.Dtos.ProductDtos;
using MongoDbProduct.Entities;
using MongoDbProduct.Settings;
using NuGet.Packaging.Signing;

namespace MongoDbProduct.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
        public async Task CreateProductAsync(CreateProductDto productDto)
        {
            var value = _mapper.Map<Product>(productDto);
            await _productCollection.InsertOneAsync(value);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(x => x.ProductId == id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var products = await _productCollection.Find(x => true).ToListAsync();

            // Tüm kategorileri çekin
            var categories = await _categoryCollection.Find(x => true).ToListAsync();

            // Ürünleri map ederken CategoryName'i doldurun
            var result = products.Select(product =>
            {
                var categoryName = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId)?.CategoryName ?? "Kategori Yok";
                return new ResultProductDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    SavedUrl = product.SavedUrl,
                    Stock = product.Stock,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    CategoryName = categoryName // CategoryName burada dolduruluyor
                };
            }).ToList();

            return result;
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var value = await _productCollection.Find<Product>(x => x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(value);
        }

        public async Task StockUpdateWhenOrderCreated(string id, int amount)
        {
            var product = await _productCollection.Find<Product>(x => x.ProductId == id).FirstOrDefaultAsync();
            product.Stock = product.Stock - amount;
            _mapper.Map<Product>(product);
            await _productCollection.FindOneAndReplaceAsync(x => x.ProductId == id, product);
        }

        public async Task UpdateProductAsync(UpdateProductDto productDto)
        {
            var value = _mapper.Map<Product>(productDto);
            await _productCollection.FindOneAndReplaceAsync(x => x.ProductId == productDto.ProductId, value);
        }
    }
}
