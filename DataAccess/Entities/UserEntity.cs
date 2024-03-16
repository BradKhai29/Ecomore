using DataAccess.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class UserEntity :
        IdentityUser<Guid>,
        IGuidEntity
    {
        public string AvatarUrl { get; set; }

        public bool Gender { get; set; }

        public Guid AccountStatusId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region Relationships
        public AccountStatusEntity AccountStatus { get; set; }

        public IEnumerable<OrderEntity> Orders { get; set; }
        #endregion

        #region MetaData
        public static class MetaData
        {
            public const string TableName = "Users";
        }
        #endregion
    }
}