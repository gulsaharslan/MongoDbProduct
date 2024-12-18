﻿namespace MongoDbProduct.Dtos.OrderDtos
{
    public class ResultOrderDto
    {
        public string OrderId { get; set; }
        public int Piece { get; set; }
        public string ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
    }
}
