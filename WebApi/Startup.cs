using Application;
using Application.Interfaces;

using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Shared;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebApi.Extensions;
using WebApi.Services;

namespace WebApi;

public class Startup
{
    public IConfiguration _config { get; }
    public Startup(IConfiguration configuration)
    {
        _config = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationLayer();
        services.AddIdentityInfrastructure(_config);
        services.AddPersistenceInfrastructure(_config);
        services.AddSharedInfrastructure(_config);
        services.AddSwaggerExtension();
        _ = services.AddControllers();
        services.AddApiVersioningExtension();
        _ = services.AddHealthChecks();
        _ = services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }
        else
        {
            _ = app.UseExceptionHandler("/Error");
            _ = app.UseHsts();
        }
        _ = app.UseHttpsRedirection();
        _ = app.UseRouting();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        app.UseSwaggerExtension();
        app.UseErrorHandlingMiddleware();
        _ = app.UseHealthChecks("/health");

        _ = app.UseEndpoints(endpoints =>
         {
             _ = endpoints.MapControllers();
         });
    }
}
