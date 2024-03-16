using DataAccess.Entities.Base;

namespace DataAccess.Entities
{
    public class OrderStatusEntity : GuidEntity
    {
        public string Name { get; set; }

        #region Relationships
        public IEnumerable<OrderEntity> Orders { get; set; }
        #endregion

        #region MetaData
        public static class MetaData
        {
            public const string TableName = "OrderStatuses";
        }
        #endregion
    }
}
