using ASI.Services.Statistics;
using ASI.Services.Statistics.Attributes;
using System;
using System.Diagnostics;

namespace WebApiTemplate.Models
{
    public class ExecutionTimeRecord : IRecord, IDisposable
    {
        private Stopwatch _stopwatch;

        public ExecutionTimeRecord()
        {
            Id = Guid.NewGuid().ToString();
            Name = typeof(ExecutionTimeRecord).Name;
            Observation = new Observation(serverGenerated: true);
            Start();
        }

        public bool Equals(IRecord other)
        {
            return other != null && string.Equals(Id, other.Id);
        }

        public string Id { get; set; }

        public string Name { get; set; }

        [Dimension]
        public string Method { get; set; }

        public Observation Observation { get; private set; }

        public void Start()
        {
            if (_stopwatch == null)
            {
                _stopwatch = Stopwatch.StartNew();
            }
            else
            {
                if (_stopwatch.IsRunning)
                {
                    _stopwatch.Stop();
                }
                _stopwatch.Reset();
                _stopwatch.Start();
            }
        }

        public void Stop()
        {
            if (_stopwatch != null && _stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                Observation = new Observation(_stopwatch.ElapsedMilliseconds, new UnitOfMeasure { Value = "ms" });
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}