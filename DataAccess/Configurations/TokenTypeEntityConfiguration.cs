using DataAccess.Commons.SqlConstants;
using DataAccess.Configurations.Base;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class TokenTypeEntityConfiguration : IEntityConfiguration<TokenTypeEntity>
    {
        public void Configure(EntityTypeBuilder<TokenTypeEntity> builder)
        {
            builder.ToTable(TokenTypeEntity.MetaData.TableName);

            builder.HasKey(tokenType => tokenType.Id);

            builder
                .Property(tokenType => tokenType.Name)
                .HasColumnType(SqlDataTypes.SqlServer.NVARCHAR_20)
                .IsRequired();

            builder
                .HasIndex(category => category.Name)
                .IsUnique();

            #region Relationships
            builder
                .HasMany(tokenType => tokenType.UserTokens)
                .WithOne(userToken => userToken.TokenType);
            #endregion
        }
    }
}
