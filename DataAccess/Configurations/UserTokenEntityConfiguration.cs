using DataAccess.Commons.SqlConstants;
using DataAccess.Configurations.Base;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class UserTokenEntityConfiguration :
        IEntityConfiguration<UserTokenEntity>
    {
        public void Configure(EntityTypeBuilder<UserTokenEntity> builder)
        {
            builder.ToTable(UserTokenEntity.MetaData.TableName);

            builder
                .Property(userToken => userToken.Id)
                .IsRequired();

            builder
                .HasIndex(userToken => userToken.Id)
                .IsUnique();

            builder
                .Property(token => token.TokenTypeId)
                .IsRequired();

            builder
                .Property(token => token.Value)
                .HasColumnType(SqlDataTypes.SqlServer.VARCHAR_32)
                .IsRequired();

            builder
                .Property(userToken => userToken.ExpiredAt)
                .HasColumnType(SqlDataTypes.SqlServer.DATETIME)
                .IsRequired();

            #region Relationships
            builder
                .HasOne(token => token.TokenType)
                .WithMany(tokenType => tokenType.UserTokens)
                .HasPrincipalKey(tokenType => tokenType.Id)
                .HasForeignKey(token => token.TokenTypeId);
            #endregion
        }
    }
}
