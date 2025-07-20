using Application.Interfaces;
using Application.Interfaces.Repositories;

using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetValue<bool>("UseInMemoryDatabase")
            ? services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ApplicationDb"))
            : services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        #region Repositories

        _ = services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
        _ = services.AddTransient(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
        _ = services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();

        #endregion Repositories
    }
}