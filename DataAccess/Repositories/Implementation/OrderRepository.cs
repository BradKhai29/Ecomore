using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class OrderRepository :
        GenericRepository<OrderEntity>,
        IOrderRepository
    {
        public OrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<OrderEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<OrderEntity>> GetAllByExpressionAsync(
            Expression<Func<OrderEntity, bool>> findExpresison,
            CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(findExpresison)
                .Select(order => new OrderEntity
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedAt,
                    OrderItems = order.OrderItems.Select(orderItem => new OrderItemEntity
                    {
                        ProductId = orderItem.ProductId,
                        Product = new ProductEntity
                        {
                            Name = orderItem.Product.Name
                        },
                        SellingPrice = orderItem.SellingPrice,
                        SellingQuantity = orderItem.SellingQuantity,
                    }),
                    Status = new OrderStatusEntity
                    {
                        Id = order.Status.Id,
                        Name = order.Status.Name,
                    },
                    OrderNote = order.OrderNote,
                    TotalPrice = order.TotalPrice,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
