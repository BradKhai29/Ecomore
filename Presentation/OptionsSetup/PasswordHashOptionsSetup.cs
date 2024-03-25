using Microsoft.Extensions.Options;
using Options.Models;

namespace Presentation.OptionsSetup;

public class PasswordHashOptionsSetup : IConfigureOptions<PasswordHashOptions>
{
    private readonly IConfiguration _configuration;

    public PasswordHashOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(PasswordHashOptions options)
    {
        _configuration
            .GetRequiredSection(PasswordHashOptions.ParentSectionName)
            .GetRequiredSection(key: PasswordHashOptions.SectionName)
            .Bind(instance: options);
    }
}
