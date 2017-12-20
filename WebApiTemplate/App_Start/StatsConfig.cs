using System.Collections.Generic;
using System.Linq;
using ASI.Services.Statistics.Data;
using ASI.Services.Statistics.Http;
using System.Web.Http;
using ASI.Services.Statistics;
using WebApi.StructureMap;
using WebApiTemplate.Filters;
using WebApiTemplate.Models;

namespace WebApiTemplate
{
    public static class StatsConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var store = config.DependencyResolver.GetService<IQuery>();

            // Enable stats collection
            var collector = store as ICollector;
            var mapper = new StatsCollectorMapper();
            config.MessageHandlers.Add(collector != null ? new StatsHandler(mapper, collector) : new StatsHandler(mapper));
            config.Filters.Add(new StatsActionFilter());

            // Enable health checks
            HealthCheckConfig.Register(config, store);
        }

        private class StatsCollectorMapper : ICollectorMapper
        {
            public IDictionary<ICollector, ICollection<IRecord>> Map(IEnumerable<ICollector> collectors, ICollection<IRecord> records)
            {
                return collectors.ToDictionary(c => c, c => Map(c, records));
            }

            private static ICollection<IRecord> Map(ICollector collector, IEnumerable<IRecord> records)
            {
                if (collector is InMemoryRecordStore)
                {
                    return records.OfType<ExecutionTimeRecord>().Cast<IRecord>().ToArray();
                }
                return records.Where(r => !(r is ExecutionTimeRecord)).ToArray();
            }
        }
    }
}