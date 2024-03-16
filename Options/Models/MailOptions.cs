namespace Options.Models
{
    public class MailOptions
    {
        public const string Link1_PlaceHolder = "{link_1}";
        public const string Link2_PlaceHolder = "{link_2}";

        public const string SectionName = "Mail";

        public string Address { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string RedirectUrl { get; set; }

        public string GetFullLink(string uri)
        {
            return $"{RedirectUrl}/{uri}";
        }
    }
}
