using MongoDbProduct.Dtos.OrderDtos;

namespace MongoDbProduct.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<ResultOrderDto>> GetAllOrderAsync();
        Task CreateOrderAsync(CreateOrderDto orderDto);
        Task UpdateOrderAsync(UpdateOrderDto orderDto);
        Task DeleteOrderAsync(string id);
        Task<GetByIdOrderDto> GetByIdOrderAsync(string id);
    }
}
