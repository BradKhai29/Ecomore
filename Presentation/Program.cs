using BusinessLogic;
using DataAccess;
using Presentation.ExtensionMethods;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

#region Core and External Services Configuration
services.AddDbContextConfiguration(configuration);
services.AddIdentityConfiguration();
services.AddDataAccess();
services.AddBusinessLogic();
#endregion

#region Presentation Configuration
services.AddOptionsConfiguration();
services.AddCustomCookieConfiguration();
services.AddWebApiConfiguration();
services.AddRazorPages();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<CustomerIdCookieMiddleware>();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
