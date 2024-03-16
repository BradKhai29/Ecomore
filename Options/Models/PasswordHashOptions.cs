using Options.Commons.Constants;

namespace Options.Models
{
    public class PasswordHashOptions
    {
        public const string ParentSectionName = AuthenticationSection.Name;
        public const string SectionName = "PasswordHash";

        public string PrivateKey { get; set; }
    }
}
