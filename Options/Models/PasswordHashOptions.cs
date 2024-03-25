using Options.Commons.Constants;

namespace Options.Models
{
    public class PasswordHashOptions
    {
        public const string ParentSectionName = AuthenticationSections.RootSection;
        public const string SectionName = AuthenticationSections.PasswordHashSection;

        public string PrivateKey { get; set; }
    }
}
