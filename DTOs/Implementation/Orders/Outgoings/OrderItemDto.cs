using System;

namespace DTOs.Implementation.Orders.Outgoings
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal => UnitPrice * Quantity;
    }
}
