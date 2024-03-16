using Microsoft.Extensions.Options;
using Options.Models;

namespace Presentation.OptionsSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        _configuration
            .GetRequiredSection(JwtOptions.ParentSectionName)
            .GetRequiredSection(key: JwtOptions.SectionName)
            .Bind(instance: options);
    }
}
