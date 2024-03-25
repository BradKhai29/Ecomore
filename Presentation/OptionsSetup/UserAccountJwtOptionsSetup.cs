using Microsoft.Extensions.Options;
using Options.Models.Jwts;

namespace Presentation.OptionsSetup;

public class UserAccountJwtOptionsSetup : IConfigureOptions<UserAccountJwtOptions>
{
    private readonly IConfiguration _configuration;

    public UserAccountJwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(UserAccountJwtOptions options)
    {
        _configuration
            .GetRequiredSection(JwtOptions.ParentSectionName)
            .GetRequiredSection(JwtOptions.UserAccountSection)
            .GetRequiredSection(key: JwtOptions.JwtSection)
            .Bind(instance: options);
    }
}
