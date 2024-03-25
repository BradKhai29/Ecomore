using DataAccess.Entities.Base;

namespace DataAccess.Entities
{
    public class BlogEntity :
        GuidEntity,
        ICreatedEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsVerifiedToUpload { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid VerifiedBy { get; set; }

        #region Relationships
        public SystemAccountEntity Creator { get; set; }

        public SystemAccountEntity Verifier { get; set; }
        #endregion
    }
}
