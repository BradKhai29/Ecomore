using DataAccess.Commons.SqlConstants;
using DataAccess.Configurations.Base;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class CategoryEntityConfiguration : IEntityConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable(CategoryEntity.MetaData.TableName);

            builder.HasKey(category => category.Id);

            builder
                .Property(category => category.Name)
                .HasColumnType(SqlDataTypes.SqlServer.NVARCHAR_50)
                .IsRequired();

            builder
                .HasIndex(category => category.Name)
                .IsUnique();

            #region Relationships
            builder
                .HasMany(category => category.Products)
                .WithOne(product => product.Category);
            #endregion
        }
    }
}
