using DataAccess.Entities.Base;

namespace DataAccess.Entities
{
    public class OrderEntity :
        GuidEntity,
        IUpdatedEntity
    {
        public Guid StatusId { get; set; }

        public Guid UserId { get; set; }

        public Guid GuestId { get; set; }

        public Guid PaymentMethodId { get; set; }

        /// <summary>
        ///     Used to store the transaction code that received from 
        ///     the <b>3rd-party online banking provider</b>
        ///     or the <b>system auto-generated</b>.
        /// </summary>
        public string TransactionCode { get; set; }

        public string OrderNote { get; set; }

        public decimal TotalPrice { get; set; }

        public string DeliveredAddress { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        
        public Guid UpdatedBy { get; set; }

        public DateTime DeliveredAt { get; set; }

        #region Relationships
        public UserEntity User { get; set; }

        public OrderGuestDetailEntity OrderGuestDetail { get; set; }

        public PaymentMethodEntity PaymentMethod { get; set; }

        public SystemAccountEntity Updater { get; set; }

        public IEnumerable<OrderItemEntity> OrderItems { get; set; }

        public OrderStatusEntity Status { get; set; }
        #endregion

        #region MetaData
        public static class MetaData
        {
            public const string TableName = "Orders";
        }
        #endregion
    }
}
