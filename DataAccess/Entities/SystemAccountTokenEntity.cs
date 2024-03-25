using DataAccess.Entities.Base;

namespace DataAccess.Entities
{
    public class SystemAccountTokenEntity : GuidEntity
    {
        public Guid SystemAccountId { get; set; }

        public Guid TokenTypeId { get; set; }

        public string Value { get; set; }

        public DateTime ExpiredAt { get; set; }

        #region Relationships
        public SystemAccountEntity SystemAccount { get; set; }

        public TokenTypeEntity TokenType { get; set; }
        #endregion

        #region MetaData
        public static class MetaData
        {
            public const string TableName = "SystemAccountTokens";
        }
        #endregion
    }
}
