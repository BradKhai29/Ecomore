using System;
using System.Collections.Generic;

namespace DTOs.Implementation.Orders.Outgoings
{
    public class GetOrderByIdDto
    {
        public Guid OrderId { get; set; }

        public Guid StatusId { get; set; }

        public string StatusName { get; set; }

        public decimal TotalPrice { get; set; }

        public int TotalItems { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<OrderItemDto> OrderItems { get; set; }
    }
}
