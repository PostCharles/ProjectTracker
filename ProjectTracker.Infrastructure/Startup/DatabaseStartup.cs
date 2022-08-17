using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.Core.Interfaces.InfrastructureServices;
using ProjectTracker.Core.Models.VaultSecrets;
using ProjectTracker.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTracker.Core.Exceptions;

namespace ProjectTracker.Infrastructure.Startup
{
    public static class DatabaseStartup
    {
        public static void RegisterContext<T>(this IServiceCollection services, Func<ConnectionStringSecret, string> getConnectionStringFromSecret) where T : DbContext
        {
            var serviceProvider = services.BuildServiceProvider();
            var vault = serviceProvider.GetRequiredService<IVault>();

            var secret = vault.GetSecretAsync<ConnectionStringSecret>().Result;

            if (secret == null) throw new SecretNotFoundException();

            var connectionString = getConnectionStringFromSecret.Invoke(secret);

            services.AddDbContext<T>(options => options.UseNpgsql(connectionString));
        }
    }
}
