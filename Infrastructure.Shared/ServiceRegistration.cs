using Application.Interfaces;

using Domain.Settings;

using Infrastructure.Shared.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Shared;

public static class ServiceRegistration
{
    public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
    {
        _ = services.Configure<MailSettings>(_config.GetSection("MailSettings"));
        _ = services.AddTransient<IDateTimeService, DateTimeService>();
        _ = services.AddTransient<IEmailService, EmailService>();
    }
}