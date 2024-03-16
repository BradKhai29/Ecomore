using System;
using System.Collections.Generic;

namespace DTOs.Implementation.Orders.Outgoings
{
    public class GetOrderByIdDto
    {
        public Guid OrderId { get; set; }

        public IEnumerable<OrderItemDto> OrderItems { get; set; }
    }
}
