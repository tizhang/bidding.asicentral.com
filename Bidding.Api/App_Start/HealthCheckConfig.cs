using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using ASI.Services.Monitoring;
using ASI.Services.Statistics.Data;
using Bidding.Api.Models;

namespace Bidding.Api
{
    public static class HealthCheckConfig
    {
        public static void Register(HttpConfiguration config, IQuery store)
        {
            HealthChecks.RegisterHealthCheck("Performance", () =>
            {
                var result = store.Filter<ExecutionTimeRecord>(typeof(ExecutionTimeRecord).Name, r => true).Result;
                return result.Select(r => (double)r.Observation.Value).DefaultIfEmpty(0.0).Average() > 1500 ? HealthCheckResult.Unhealthy() : HealthCheckResult.Healthy();
            });

            HealthChecks.RegisterHealthCheck("Database Connectivity", () =>
            {
                for (var i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
                {
                    var configElement = ConfigurationManager.ConnectionStrings[i];

                    if (!string.IsNullOrWhiteSpace(configElement.Name) && !string.Equals(configElement.Name, "LocalSqlServer") &&  string.Equals(configElement.ProviderName, "System.Data.SqlClient", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using (var connection = new SqlConnection(configElement.ConnectionString))
                            {
                                connection.Open();
                            }
                        }
                        catch (Exception ex)
                        {
                            return HealthCheckResult.Unhealthy(ex);
                        }
                    }
                }

                return HealthCheckResult.Healthy();
            });
        }
    }
}