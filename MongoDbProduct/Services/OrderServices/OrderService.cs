using AutoMapper;
using MongoDB.Driver;
using MongoDbProduct.Dtos.OrderDtos;
using MongoDbProduct.Dtos.ProductDtos;
using MongoDbProduct.Entities;
using MongoDbProduct.Settings;

namespace MongoDbProduct.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<Order> _orderCollection;
        private readonly IMongoCollection<Customer> _customerCollection;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;
        public OrderService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _orderCollection = database.GetCollection<Order>(_databaseSettings.OrderCollectionName);
            _customerCollection = database.GetCollection<Customer>(_databaseSettings.CustomerCollectionName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _mapper = mapper;
        }
        public async Task CreateOrderAsync(CreateOrderDto orderDto)
        {
            var value = _mapper.Map<Order>(orderDto);
            await _orderCollection.InsertOneAsync(value);
        }

        public async Task DeleteOrderAsync(string id)
        {
            await _orderCollection.DeleteOneAsync(x => x.OrderId == id);
        }

        public async Task<List<ResultOrderDto>> GetAllOrderAsync()
        {
            var orders = await _orderCollection.Find(x => true).ToListAsync();
            var customers = await _customerCollection.Find(x => true).ToListAsync();
            var products = await _productCollection.Find(x => true).ToListAsync();

            var result = orders.Select(order =>
            {
                var customer = customers.FirstOrDefault(c => c.CustomerId == order.CustomerId);
                var product = products.FirstOrDefault(p => p.ProductId == order.ProductId);
                return new ResultOrderDto
                {
                    OrderId = order.OrderId,
                    CustomerName = customer != null ? customer.CustomerName : "Bilinmiyor",
                    ProductName = product != null ? product.ProductName : "Bilinmiyor",
                    Piece = order.Piece,
                    OrderDate = order.OrderDate
                };
            }).ToList();

            return result;

        }

        public async Task<GetByIdOrderDto> GetByIdOrderAsync(string id)
        {
            var value = await _orderCollection.Find(x => x.OrderId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdOrderDto>(value);
        }

        public async Task UpdateOrderAsync(UpdateOrderDto orderDto)
        {
            var values = _mapper.Map<Order>(orderDto);
            await _orderCollection.FindOneAndReplaceAsync(x => x.OrderId == orderDto.OrderId, values);
        }
    }
}
