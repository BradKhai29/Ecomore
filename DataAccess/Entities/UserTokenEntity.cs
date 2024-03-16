using DataAccess.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class UserTokenEntity :
        IdentityUserToken<Guid>,
        IGuidEntity
    {
        public Guid Id { get; set; }

        public Guid TokenTypeId { get; set; }

        public DateTime ExpiredAt { get; set; }

        #region Relationships
        public TokenTypeEntity TokenType { get; set; }
        #endregion

        #region MetaData
        public static class MetaData
        {
            public const string TableName = "UserTokens";
        }
        #endregion
    }
}
