using AutoMapper;
using MongoDB.Driver;
using MongoDbProduct.Dtos.CustomerDtos;
using MongoDbProduct.Entities;
using MongoDbProduct.Settings;

namespace MongoDbProduct.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IMongoCollection<Customer> _customerCollection;
        private readonly IMapper _mapper;
        public CustomerService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _customerCollection = database.GetCollection<Customer>(_databaseSettings.CustomerCollectionName);
            _mapper = mapper;
        }
        public async Task CreateCustomerAsync(CreateCustomerDto customerDto)
        {
            var value = _mapper.Map<Customer>(customerDto);
            await _customerCollection.InsertOneAsync(value);
        }

        public async Task DeleteCustomerAsync(string id)
        {
            await _customerCollection.DeleteOneAsync(x => x.CustomerId == id);
        }

        public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
        {
            var values = await _customerCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCustomerDto>>(values);
        }

        public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(string id)
        {
            var value = await _customerCollection.Find(i => i.CustomerId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdCustomerDto>(value);
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto customerDto)
        {
            var value = _mapper.Map<Customer>(customerDto);
            await _customerCollection.FindOneAndReplaceAsync(x => x.CustomerId == customerDto.CustomerId, value);
        }
    }
}
