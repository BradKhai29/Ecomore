using Microsoft.Extensions.Options;
using Options.Models;

namespace Presentation.OptionsSetup
{
    public class MailOptionsSetup : IConfigureOptions<MailOptions>
    {
        private readonly IConfiguration _configuration;

        public MailOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MailOptions options)
        {
            _configuration
                .GetRequiredSection(MailOptions.SectionName)
                .Bind(options);
        }
    }
}
