using System;
using Microsoft.Extensions.Configuration;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Startup
{
    internal sealed class ServiceFactory
    {
        private readonly IConfigurationRoot _configuration;

        public ServiceFactory()
        {
            var environment =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? "Development";

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true,
                    reloadOnChange: false
                )
                .AddEnvironmentVariables()
                .Build();
        }

        public IQuantityMeasurementService CreateService()
        {
            return new QuantityMeasurementServiceImpl();
        }

        public IQuantityMeasurementRepository CreateRepository()
        {
            var provider =
                Environment.GetEnvironmentVariable("QMA_PERSISTENCE_PROVIDER")
                ?? _configuration["Persistence:Provider"]
                ?? "Cache";

            if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("QMA_SQLSERVER_CONNECTION_STRING")
                    ?? _configuration["Database:ConnectionString"];

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "Connection string is missing. Set Database:ConnectionString in appsettings or QMA_SQLSERVER_CONNECTION_STRING."
                    );
                }

                try
                {
                    return new QuantityMeasurementDatabaseRepository(connectionString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Warning: SQL Server unavailable ({ex.Message}). Falling back to in-memory cache repository."
                    );
                    return QuantityMeasurementCacheRepository.Instance;
                }
            }

            return QuantityMeasurementCacheRepository.Instance;
        }
    }
}
