using Microsoft.Extensions.Options;
using Options.Models;

namespace Presentation.OptionsSetup
{
    public class ResetPasswordOptionsSetup : IConfigureOptions<ResetPasswordOptions>
    {
        private readonly IConfiguration _configuration;

        public ResetPasswordOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(ResetPasswordOptions options)
        {
            _configuration
                .GetRequiredSection(ResetPasswordOptions.ParentSectionName)
                .GetRequiredSection(ResetPasswordOptions.SectionName)
                .Bind(options);
        }
    }
}
