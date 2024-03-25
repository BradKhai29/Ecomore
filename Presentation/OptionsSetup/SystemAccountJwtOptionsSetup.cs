using Microsoft.Extensions.Options;
using Options.Models;
using Options.Models.Jwts;

namespace Presentation.OptionsSetup;

public class SystemAccountJwtOptionsSetup : IConfigureOptions<SystemAccountJwtOptions>
{
    private readonly IConfiguration _configuration;

    public SystemAccountJwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SystemAccountJwtOptions options)
    {
        _configuration
            .GetRequiredSection(JwtOptions.ParentSectionName)
            .GetRequiredSection(JwtOptions.SystemAccountSection)
            .GetRequiredSection(key: JwtOptions.JwtSection)
            .Bind(instance: options);
    }
}
